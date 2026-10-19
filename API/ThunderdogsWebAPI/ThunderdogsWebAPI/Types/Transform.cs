using RosSharp.RosBridgeClient;
using System.Security.Cryptography.Xml;

namespace ThunderdogsWebAPI.Types {
    public class Transform : Message {
        public const string RosMessageName = "geometry_msgs/msg/Transform";

        public Vector3 translation { get; set; }
        public Quaternion rotation { get; set; }

        public Transform() {

        }

        public Transform(Vector3 translation, Quaternion rotation) {
            this.translation = translation;
            this.rotation = rotation;
        }
    }
}
