using Microsoft.AspNetCore.Mvc;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Controllers
{
    [Route("api")]
    [ApiController]
    public class RedeemauthoritiesController : ControllerBase
    {
        private readonly IRedeemauthoritiesRepository _redeemauthoritiesRepository;
        public RedeemauthoritiesController(IRedeemauthoritiesRepository redeemauthoritiesRepository)
        {
            _redeemauthoritiesRepository = redeemauthoritiesRepository;
        }

        [HttpPost]
        [Route("redeemauthorities")]
        public IActionResult Post(RedeemauthoritiesModel model)
        {
            dynamic response = _redeemauthoritiesRepository.InsertRedeemAuthorities(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("redeemauthorities/{id=0}")]
        public IActionResult Get(int id, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _redeemauthoritiesRepository.GetRedeemAuthorities(id, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("redeemauthorities")]
        public IActionResult Put(RedeemauthoritiesModel model)
        {
            dynamic response = _redeemauthoritiesRepository.UpdateRedeemAuthorities(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("redeemauthorities/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic response = _redeemauthoritiesRepository.DeleteRedeemAuthorities(id);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
