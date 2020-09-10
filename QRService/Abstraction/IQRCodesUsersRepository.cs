using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Abstraction
{
    public interface IQRCodesUsersRepository
    {
        dynamic GetQrcodesUsers(int qrCodesUsersId, Pagination pageInfo);
        dynamic UpdateQrcodesUsers(QrcodesusersModel model);
        dynamic DeleteQrcodesUsers(int id);
        dynamic InsertQrcodesUsers(QrcodesusersModel model);
    }
}
