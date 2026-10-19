using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace ThunderdogsWebAPI.Types
{
    public class Image : Message {
        public const string RosMessageName = "sensor_msgs/msg/Image";

        public Header header { get; set; }
        public UInt32 height { get; set; }
        public UInt32 width { get; set; }
        public string encoding { get; set; }
        public sbyte is_bigendian { get; set; }
        public UInt32 step { get; set; }
        public sbyte[] data { get; set; }

        public Image() {

        }

        public Image(Header header, UInt32 height, UInt32 width, string encoding, sbyte is_bigendian, UInt32 step, sbyte[] data) {
            this.header = header;
            this.height = height;
            this.width = width;
            this.encoding = encoding;
            this.is_bigendian = is_bigendian;
            this.step = step;
            this.data = data;
        }
    }
}
