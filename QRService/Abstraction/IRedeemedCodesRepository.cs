using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Abstraction
{
    public interface IRedeemedCodesRepository
    {
        dynamic GetRedeemedCodes(int redeemedCodesId, Pagination pageInfo);
        dynamic UpdateRedeemedCodes(RedeemedcodesModel model);
        dynamic DeleteRedeemedCodes(int id);
        dynamic InsertRedeemedCodes(RedeemedcodesModel model);
    }
}
