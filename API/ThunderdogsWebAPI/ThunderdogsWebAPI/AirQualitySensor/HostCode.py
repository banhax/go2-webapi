import csv
import json
import time
import os
from datetime import date, datetime, timedelta

import serial
from serial.tools import list_ports

CONFIG_PATH = "config.json"
LOG_DIR = "logs"
LOG_EVERY = 60
PICO_VID = 0x2E8A
WARMUP_SECONDS = 5 * 60
CALIBRATE_DUE = timedelta(days=60)
CALIBRATE_TIMEOUT = 180

warmed = False


def load_config():
    if os.path.exists(CONFIG_PATH):
        try:
            with open(CONFIG_PATH) as f:
                return json.load(f)
        except ValueError:
            print("config.json unreadable, treating as uncalibrated")
    return {}

def save_config(cfg):
    tmp = CONFIG_PATH + ".tmp"
    with open(tmp, "w") as f:
        json.dump(cfg, f, indent=2)
    os.replace(tmp, CONFIG_PATH)

class ReadingLogger:
    def __init__(self, log_dir=LOG_DIR, batch_size=LOG_EVERY):
        self.log_dir = log_dir
        self.batch_size = batch_size
        self.readings = []
        os.makedirs(self.log_dir, exist_ok=True)

    def _path_for(self, day):
        return os.path.join(self.log_dir, f"sensor_{day.isoformat()}.csv")

    def add(self, ppm):
        self.readings.append(ppm)
        if len(self.readings) >= self.batch_size:
            self._write_average()

    def _write_average(self):
        now = datetime.now()
        avg = sum(self.readings) / len(self.readings)
        path = self._path_for(now.date())
        try:
            new_file = not os.path.exists(path)   # create daily log if missing
            with open(path, "a", newline="") as f:
                writer = csv.writer(f)
                if new_file:
                    writer.writerow(["timestamp", "avg_ppm"])
                writer.writerow([now.isoformat(timespec="seconds"), f"{avg:.1f}"])
        except OSError as e:
            print(f"Could not write log: {e}")    # keep readings, retry next time
            return
        self.readings.clear()        
   

def calibration_due(cfg):
    if "r0" not in cfg or "last_calibrated" not in cfg:
        return True
    last = date.fromisoformat(cfg["last_calibrated"])
    return date.today() - last > CALIBRATE_DUE


def find_pico():
    for p in list_ports.comports():        
        if p.vid == PICO_VID:            
            return p.device
    return None

def send(ser, obj):
    ser.write((json.dumps(obj) + "\n").encode())

def read_event(ser):
    line = ser.readline().decode(errors="replace").strip()
    if not line:
        return None
    if line.startswith("{"):
        try:
            return json.loads(line)
        except ValueError:
            pass
    print("Pico:", line)
    return None


def warm_up(ser):
    global warmed
    print(f"Warming sensor for {WARMUP_SECONDS // 60} min...")
    end = time.monotonic() + WARMUP_SECONDS
    while time.monotonic() < end:
        read_event(ser)
    warmed = True
    print("Warm-up complete")

def calibrate(ser, cfg):
    print("Calibration due, sending command (keep sensor in clean air)")
    ser.reset_input_buffer()
    send(ser, {"cmd":"calibrate"})
    end = time.monotonic() + CALIBRATE_TIMEOUT
    while time.monotonic() < end:
        event = read_event(ser)
        if event and event.get("event") == "calibrate_done":
            cfg["r0"] = event["r0"]
            cfg["last_calibrated"] = date.today().isoformat()
            save_config(cfg)
            print(f"Calibration finished, R0 = {cfg['r0']:.2f} kOhm")
            return
    raise TimeoutError("Pico did not finish calibration in time")

def push_calibration(ser, cfg):
    print(f"Calibration OK (last: {cfg['last_calibrated']}), sending R0={cfg['r0']:.2f}")
    send(ser, {"cmd": "set_r0", "r0": cfg["r0"]})

def run(port, logger):
    with serial.Serial(port, 115200, timeout=1) as ser:
        print(f"Connected on {port}")
        ser.reset_input_buffer()
 
        if not warmed:
            warm_up(ser)
 
        cfg = load_config()
        if calibration_due(cfg):
            calibrate(ser, cfg)
            send(ser, {"cmd": "set_r0", "r0": cfg["r0"]})
        else:
            push_calibration(ser, cfg)
         
        while True:
            event = read_event(ser)
            if event and event.get("event") == "reading":
                print(f"{event['status']}: {event['ppm']:.0f} ppm")
                logger.add(event["ppm"])
                # TODO: publish to main dashboard
 
 
def main():
    logger = ReadingLogger()
    while True:
        port = find_pico()
        if port is None:
            print("Scanning for sensor...")
            time.sleep(1)
            continue
        try:
            run(port, logger)
        except (serial.SerialException, OSError):
            print("Disconnected, waiting for Pico...")
            time.sleep(1)
        except TimeoutError as e:
            print(e)
            time.sleep(5)
 
 
if __name__ == "__main__":
    main()
