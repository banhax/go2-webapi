using RosSharp.RosBridgeClient;
using ThunderdogsWebAPI.Types;

namespace ThunderdogsWebAPI.Interfaces {
    public interface IRosService {
        public string Publish(string topic, string payload);
        public Task<T> SubscribeAsync<T>(string topic) where T : Message;
    }
}
