using RosSharp.RosBridgeClient;

namespace ThunderdogsWebAPI.Types
{
    public class Vector3 : Message {
        public const string RosMessageName = "geometry_msgs/msg/Vector3";

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Vector3()
        {

        }

        public Vector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
