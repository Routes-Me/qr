using Microsoft.AspNetCore.Mvc;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Controllers
{
    [Route("api")]
    [ApiController]
    public class QRCodesAdvertisementsController : ControllerBase   
    {
        private readonly IQRCodesAdvertisementsRepository _qrCodesAdvertisementsRepository;
        public QRCodesAdvertisementsController(IQRCodesAdvertisementsRepository qrCodesAdvertisementsRepository)
        {
            _qrCodesAdvertisementsRepository = qrCodesAdvertisementsRepository;
        }

        [HttpPost]
        [Route("qrcodesadvertisements")]
        public IActionResult Post(QrcodesadvertismentsModel model)
        {
            dynamic response = _qrCodesAdvertisementsRepository.InsertQrcodesAdvertisments(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("qrcodesadvertisements/{qrCodesAdvertisementsId=0}")]
        public IActionResult Get(int qrCodesAdvertisementsId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _qrCodesAdvertisementsRepository.GetQrcodesAdvertisments(qrCodesAdvertisementsId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("qrcodesadvertisements")]
        public IActionResult Put(QrcodesadvertismentsModel model)
        {
            dynamic response = _qrCodesAdvertisementsRepository.UpdateQrcodesAdvertisments(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("qrcodesadvertisements/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic response = _qrCodesAdvertisementsRepository.DeleteQrcodesAdvertisments(id);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
