using Microsoft.AspNetCore.Mvc;
using ThunderdogsWebAPI.Interfaces;
using ThunderdogsWebAPI.Types;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;

namespace ThunderdogsWebAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CameraFeedController : ControllerBase {
        //private IRosService rosService;

        //public CameraFeedController(IRosService ros) {
        //    rosService = ros;
        //}

        [HttpGet(Name = "GetCameraFeed")]
        public string Get() {
            return RosService.Publish("foob", "aaaahh");
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            //do task!
            var data = await RosService.SubscribeAsync<Image>("/camera/raw_image");

            return Ok(new { data = $"{data}" });
        }
    }
}
