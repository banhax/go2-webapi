using RosSharp.RosBridgeClient;
using ThunderdogsWebAPI.Types;

namespace ThunderdogsWebAPI.Interfaces {
    public interface IRosService {
        public void Publish(string topic, string payload);
        public Task<ConnectedClients> SubscribeAsync<T>(string topic) where T : Message;
    }
}
