using Microsoft.AspNetCore.Mvc;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Controllers
{
    [Route("api")]
    [ApiController]
    public class RedeemedCodesController : ControllerBase
    {
        private readonly IRedeemedCodesRepository _redeemedCodesRepository;
        public RedeemedCodesController(IRedeemedCodesRepository redeemedCodesRepository)
        {
            _redeemedCodesRepository = redeemedCodesRepository;
        }

        [HttpPost]
        [Route("redeemedcodes")]
        public IActionResult Post(RedeemedcodesModel model)
        {
            dynamic response = _redeemedCodesRepository.InsertRedeemedCodes(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("redeemedcodes/{redeemedCodesId=0}")]
        public IActionResult Get(int redeemedCodesId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _redeemedCodesRepository.GetRedeemedCodes(redeemedCodesId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("redeemedcodes")]
        public IActionResult Put(RedeemedcodesModel model)
        {
            dynamic response = _redeemedCodesRepository.UpdateRedeemedCodes(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("redeemedcodes/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic response = _redeemedCodesRepository.DeleteRedeemedCodes(id);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
