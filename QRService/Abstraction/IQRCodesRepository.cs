using QRService.Models;
using QRService.Models.ResponseModel;
using System.Threading.Tasks;

namespace QRService.Abstraction
{
    public interface IQRCodesRepository
    {
        Task<dynamic> InsertQRCode(QrcodesModel model);
        Task<dynamic> UpdateQRCode(QrcodesModel model);
        Task<dynamic> DeleteQRCode(int id);
        dynamic GetQRCode(int qrCodeId, Pagination pageInfo);
        dynamic GetAdvertisementQRCode(int id, Pagination pageInfo);
    }
}
