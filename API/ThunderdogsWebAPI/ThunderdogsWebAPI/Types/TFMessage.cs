using RosSharp.RosBridgeClient;

namespace ThunderdogsWebAPI.Types {
    public class TFMessage : Message {
        public const string RosMessageName = "tf2_msgs/msg/TFMessage";

        public TransformStamped[] transforms { get; set; }

        public TFMessage() {
        
        }

        public TFMessage(TransformStamped[] transforms) {
            this.transforms = transforms;
        }
    }
}
