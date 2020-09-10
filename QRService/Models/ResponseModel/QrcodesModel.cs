using System;

namespace QRService.Models.ResponseModel
{
    public class QrcodesModel
    {
        public int QrCodeId { get; set; }
        public string ResourceName { get; set; }
        public string Details { get; set; }
        public int? InstitutionId { get; set; }
        public int? RedeemLimit { get; set; }
        public DateTime? ExpieryDate { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GetQrcodesModel
    {
        public int? AdvertisementId { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
    }
}
