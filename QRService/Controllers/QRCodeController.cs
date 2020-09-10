using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Controllers
{
    [Route("api")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRCodesRepository _qrRepository;
        public QRCodeController(IQRCodesRepository qrRepository)
        {
            _qrRepository = qrRepository;
        }

        [HttpPost]
        [Route("qr")]
        public async Task<IActionResult> Post(QrcodesModel model)
        {
            dynamic response = await _qrRepository.InsertQRCode(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("qr/{id=0}")]
        public IActionResult Get(int id, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _qrRepository.GetQRCode(id, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("advertisementqr/{id=0}")]
        public IActionResult GetAdvertisementQRCode(int id, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _qrRepository.GetAdvertisementQRCode(id, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("qr")]
        public async Task<IActionResult> Put(QrcodesModel model)
        {
            dynamic response = await _qrRepository.UpdateQRCode(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("qr/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            dynamic response = await _qrRepository.DeleteQRCode(id);
            return StatusCode((int)response.statusCode, response);
        }

    }
}
