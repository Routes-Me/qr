using QRService.Models;
using QRService.Models.ResponseModel;

namespace QRService.Abstraction
{
    public interface IRedeemauthoritiesRepository
    {
        dynamic GetRedeemAuthorities(int redeemAuthoritiesId, Pagination pageInfo);
        dynamic UpdateRedeemAuthorities(RedeemauthoritiesModel model);
        dynamic DeleteRedeemAuthorities(int id);
        dynamic InsertRedeemAuthorities(RedeemauthoritiesModel model);
    }
}
