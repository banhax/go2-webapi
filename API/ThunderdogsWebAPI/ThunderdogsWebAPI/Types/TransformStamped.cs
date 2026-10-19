using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using System.Transactions;

namespace ThunderdogsWebAPI.Types {
    public class TransformStamped : Message {
        public const string RosMessageName = "geometry_msgs/msg/TransformStamped";

        public Header header { get; set; }
        public string child_frame_id { get; set; }
        public Transform transform { get; set; }

        public TransformStamped()
        {

        }

        public TransformStamped(Header header, string child_frame_id, Transform transform)
        {
            this.header = header;
            this.child_frame_id = child_frame_id;
            this.transform = transform;
        }
    }
}
