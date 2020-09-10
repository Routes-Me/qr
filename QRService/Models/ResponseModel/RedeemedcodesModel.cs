using System;

namespace QRService.Models.ResponseModel
{
    public class RedeemedcodesModel
    {
        public int RedeemedCodeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public int? QrCodeId { get; set; }
    }
}
