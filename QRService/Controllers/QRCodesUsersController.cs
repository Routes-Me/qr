using Microsoft.AspNetCore.Mvc;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Controllers
{
    [Route("api")]
    [ApiController]
    public class QRCodesUsersController : ControllerBase
    {
        private readonly IQRCodesUsersRepository _qrCodesUsersRepository;
        public QRCodesUsersController(IQRCodesUsersRepository qrCodesUsersRepository)
        {
            _qrCodesUsersRepository = qrCodesUsersRepository;
        }

        [HttpPost]
        [Route("qrcodesusers")]
        public IActionResult Post(QrcodesusersModel model)
        {
            dynamic response = _qrCodesUsersRepository.InsertQrcodesUsers(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("qrcodesusers/{qrCodesUsersId=0}")]
        public IActionResult Get(int qrCodesUsersId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _qrCodesUsersRepository.GetQrcodesUsers(qrCodesUsersId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("qrcodesusers")]
        public IActionResult Put(QrcodesusersModel model)
        {
            dynamic response = _qrCodesUsersRepository.UpdateQrcodesUsers(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("qrcodesusers/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic response = _qrCodesUsersRepository.DeleteQrcodesUsers(id);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
