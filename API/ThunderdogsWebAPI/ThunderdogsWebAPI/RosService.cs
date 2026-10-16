using RosSharp.RosBridgeClient;
using ThunderdogsWebAPI.Interfaces;
using ThunderdogsWebAPI.Types;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;

namespace ThunderdogsWebAPI {
    public class RosService : IRosService {
        // the rosbridge server to connect to
        string URI { get => "ws://192.168.167.134:9090"; }

        RosSocket rosSocket;

        // self explanatory
        public RosService() {
            rosSocket = new(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(URI));
        }


        // close the socket on destruction
        ~RosService()
        {
            rosSocket.Close();
        }

        // method for publishing to rosbridge, this will just publish strings
        // i think we will need overloads for different types (to publish different ros2 topic objects)
        public string Publish(string topic, string payload)
        {
            std_msgs.String message = new std_msgs.String
            {
                data = payload
            };

            string publication_id = rosSocket.Advertise<std_msgs.String>(topic);

            rosSocket.Publish(publication_id, message);

            return ($"PUBLISH TO ROS2" +
                              $"\n  topic : {publication_id}" +
                              $"\n  data : {message.data}");
        }

        // subscription stuff, idk
        public Task<T> SubscribeAsync<T>(string topic) where T : Message 
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            string subscription_id = rosSocket.Subscribe<T>($"{topic}", message => SubscriptionHandler(message, tcs));
            return tcs.Task;
        }

        private void SubscriptionHandler<T>(T message, TaskCompletionSource<T> tcs)
        {
            Console.WriteLine("Received message: " + message);
            tcs.TrySetResult(message);
        }
    }
}
