from machine import Pin, ADC
import utime
import sys
import select
import json
from math import log10
 
green = Pin(13, Pin.OUT)
yellow = Pin(12, Pin.OUT)
red = Pin(11, Pin.OUT)
 
mq2_A = ADC(27)
 
RL_VAL = 10.0
RO_CAF = 9.83
R0 = None  # set by the host (set_r0) or by calibrate()
 
min_ppm = 30
max_ppm = 400
 
# Non-blocking check for commands arriving over USB serial
poll = select.poll()
poll.register(sys.stdin, select.POLLIN)
 
 
def send(obj):
    print(json.dumps(obj))
 
 
def leds(g, y, r):
    green.value(g)
    yellow.value(y)
    red.value(r)
 
 
def read_rs(samples=100):
    total = 0
    for _ in range(samples):
        total += mq2_A.read_u16()
        utime.sleep_ms(10)
    v_out = (total / samples) * (3.3 / 65535.0)
    if v_out <= 0:
        return None
    return ((3.3 * RL_VAL) / v_out) - RL_VAL
 
 
def calibrate():
    global R0
    leds(0, 1, 0)  # yellow while calibrating
    rs_air = 0
    for _ in range(50):
        rs = read_rs()
        if rs is None:
            continue
        rs_air += rs
        utime.sleep(0.1)
    R0 = (rs_air / 50) / RO_CAF
    send({"event": "calibrate_done", "r0": R0})
 
 
def calc_ppm(ratio):
    if ratio <= 0:
        return 0
    m = -0.45
    b = 1.3
    return pow(10, ((log10(ratio) - b) / m))
 
 
def handle_commands():
    global R0
    while poll.poll(0):
        line = sys.stdin.readline().strip()
        if not line.startswith("{"):
            continue
        try:
            msg = json.loads(line)
        except ValueError:
            continue
        cmd = msg.get("cmd")
        if cmd == "calibrate":
            calibrate()
        elif cmd == "set_r0":
            R0 = float(msg["r0"])
            send({"event": "r0_set", "r0": R0})
 
 
send({"event": "hello"})
 
while True:
    handle_commands()
 
    if R0 is None:
        # Not calibrated yet: wait for the host to calibrate or send R0
        leds(0, 1, 0)
        utime.sleep_ms(200)
        continue
 
    rs = read_rs()
    if rs is None:
        continue
    ppm = calc_ppm(rs / R0)
 
    if ppm < min_ppm:
        leds(1, 0, 0)
        status = "Safe"
    elif ppm < max_ppm:
        leds(0, 1, 0)
        status = "Caution"
    else:
        leds(0, 0, 1)
        status = "Danger"
 
    send({"event": "reading", "ppm": ppm, "status": status})
    utime.sleep(1)



