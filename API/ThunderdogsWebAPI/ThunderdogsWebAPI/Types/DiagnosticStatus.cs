using RosSharp.RosBridgeClient;
using static ThunderdogsWebAPI.Types.DiagnosticStatus;
using Header = RosSharp.RosBridgeClient.MessageTypes.Std.Header;
namespace ThunderdogsWebAPI.Types {
    public class DiagnosticArray : Message {
        public const string RosMessageName = "diagnostic_msgs/msg/DiagnosticArray";

        public Header header { get; set; }
        public DiagnosticStatus[] status { get; set; }

        public DiagnosticArray() { 
        
        }

        public DiagnosticArray (Header header, DiagnosticStatus[] status) {
            this.header = header;
            this.status = status;
        }
    }

    public class DiagnosticStatus : Message {
        public const string RosMessageName = "diagnostic_msgs/msg/DiagnosticStatus";

        //public enum Level {
        //    OK,
        //    WARN,
        //    ERROR,
        //    STALE
        //}

        public byte OK = 0;
        public byte WARN = 1;
        public byte ERROR = 2;
        public byte STALE = 3;
        public byte level { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public string hardware_id { get; set; }
        public KeyValue[] values { get; set; }

        public DiagnosticStatus()
        {

        }

        public DiagnosticStatus(byte level, string name, string message, string hardware_id, KeyValue[] values) {
            this.level = level;
            this.name = name;
            this.message = message;
            this.hardware_id = hardware_id;
            this.values = values;
        }
    }

    public class KeyValue : Message {
        public const string RosMessageName = "diagnostic_msgs/msg/KeyValue";

        public string key { get; set; }
        public string value { get; set; }

        public KeyValue()
        {

        }

        public KeyValue(string key, string value) {
            this.key = key;
            this.value = value;
        }
    }
}
