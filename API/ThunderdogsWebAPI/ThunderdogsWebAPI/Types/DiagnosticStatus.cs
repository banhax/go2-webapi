using RosSharp.RosBridgeClient;
using static ThunderdogsWebAPI.Types.DiagnosticStatus;
using Header = RosSharp.RosBridgeClient.MessageTypes.Std.Header;
namespace ThunderdogsWebAPI.Types {
    public class DiagnosticArray : Message {
        public const string RosMessageName = "diagnostic_msgs/msg/DiagnosticArray";

        Header header { get; set; }
        DiagnosticStatus[] status { get; set; }

        public DiagnosticArray (Header header, DiagnosticStatus[] status) {
            this.header = header;
            this.status = status;
        }
    }
    public class DiagnosticStatus : Message {
        public const string RosMessageName = "diagnostic_msgs/msg/DiagnosticStatus";

        public enum Level {
            OK,
            WARN,
            ERROR,
            STALE
        }

        public Level level { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public string hardware_id { get; set; }
        public Dictionary<string, string> values { get; set; }

        public DiagnosticStatus(Level level, string name, string message, string hardware_id, Dictionary<string, string> values) {
            this.level = level;
            this.name = name;
            this.message = message;
            this.hardware_id = hardware_id;
            this.values = values;
        }
    }
}
