using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.BuiltinInterfaces;

namespace ThunderdogsWebAPI.Types
{
    public class ConnectedClients : Message
    {
        public const string RosMessageName = "rosbridge_msgs/msg/ConnectedClients";
        public ConnectedClient[] clients { get; set; } // Array of connected clients

        public ConnectedClients()
        {

        }

        public ConnectedClients(ConnectedClient[] clients)
        {
            this.clients = clients;
        }
    }

    public class ConnectedClient : Message
    {
        public const string RosMessageName = "rosbridge_msgs/msg/ConnectedClient";

        public string ip_address { get; set; }
        public Time connection_time { get; set; }

        public ConnectedClient()
        {

        }

        public ConnectedClient(string ip_address, Time connection_time)
        {
            this.ip_address = ip_address;
            this.connection_time = connection_time;
        }
    }
}
