using ThunderdogsWebAPI.Interfaces;
using ThunderdogsWebAPI.Types;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.Protocols;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ThunderdogsWebAPI {
    public static class RosService {
        // the rosbridge server to connect to
        static string URI { get => "ws://192.168.167.134:8765"; }

        static RosSocket rosSocket;

        // self explanatory
        //public static RosService() {
        //    StartRosService();
        //}

        public async static void StartRosService() {
            rosSocket = new RosSocket(new WebSocketNetProtocol(URI));
            await rosSocket.Connected;
        }


        // close the socket on destruction
        //~RosService() {
        //    rosSocket.Close();
        //}

        // method for publishing to rosbridge, this will just publish strings
        // i think we will need overloads for different types (to publish different ros2 topic objects)
        public static string Publish(string topic, string payload) {
            std_msgs.String message = new std_msgs.String {
                data = payload
            };

            string publication_id = rosSocket.Advertise<std_msgs.String>(topic);

            rosSocket.Publish(publication_id, message);

            return ($"PUBLISH TO ROS2" +
                              $"\n  topic : {publication_id}" +
                              $"\n  data : {message.data}");
        }

        // subscription stuff, idk
        public static Task<T> SubscribeAsync<T>(string topic) where T : Message {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            string subscription_id = rosSocket.Subscribe<T>($"{topic}", message => SubscriptionHandler(message, tcs));

            return tcs.Task;
        }

        private static void SubscriptionHandler<T>(T message, TaskCompletionSource<T> tcs) {
            //Console.WriteLine("Received message: " + message);

            string jsonString = JsonConvert.SerializeObject(message);

            Console.WriteLine($"{jsonString}\n");

            tcs.TrySetResult(message);
        }
    }
}
