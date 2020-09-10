using System;

namespace QRService.Models.DBModels
{
    public partial class Redeemedcodes
    {
        public int RedeemedCodeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public int? QrCodeId { get; set; }

        public virtual Qrcodes QrCode { get; set; }
    }
}
