using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Abstraction
{
    public interface IQRCodesAdvertisementsRepository
    {
        dynamic GetQrcodesAdvertisments(int qrCodesAdvertisementsId, Pagination pageInfo);
        dynamic UpdateQrcodesAdvertisments(QrcodesadvertismentsModel model);
        dynamic DeleteQrcodesAdvertisments(int id);
        dynamic InsertQrcodesAdvertisments(QrcodesadvertismentsModel model);
    }
}
