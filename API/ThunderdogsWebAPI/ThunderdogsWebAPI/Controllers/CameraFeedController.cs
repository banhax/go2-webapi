using Microsoft.AspNetCore.Mvc;
using ThunderdogsWebAPI.Interfaces;
using ThunderdogsWebAPI.Types;

namespace ThunderdogsWebAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CameraFeedController : ControllerBase {
        private IRosService rosService;

        public CameraFeedController(IRosService ros) {
            rosService = ros;
        }

        [HttpGet(Name = "GetCameraFeed")]
        public string Get() {
            rosService.Publish("foob", "aaaahh");

            return "HelloWorld";
        }

        [HttpPost]
        public async Task<IActionResult> Callback() {
            //do task!
            var data = await rosService.SubscribeAsync<ConnectedClients>("/connected_clients");
            
            return Ok(new { data = $"{data}" });
        }
    }
}
