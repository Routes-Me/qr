using Microsoft.AspNetCore.Mvc;

namespace QRService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "QR service started successfully";
        }
    }
}
