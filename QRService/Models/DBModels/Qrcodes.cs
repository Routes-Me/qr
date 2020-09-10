using System;
using System.Collections.Generic;

namespace QRService.Models.DBModels
{
    public partial class Qrcodes
    {
        public Qrcodes()
        {
            Qrcodesadvertisments = new HashSet<Qrcodesadvertisments>();
            Qrcodesusers = new HashSet<Qrcodesusers>();
            Redeemedcodes = new HashSet<Redeemedcodes>();
        }

        public int QrCodeId { get; set; }
        public string ResourceName { get; set; }
        public string Details { get; set; }
        public int? InstitutionId { get; set; }
        public int? RedeemLimit { get; set; }
        public DateTime? ExpieryDate { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Qrcodesadvertisments> Qrcodesadvertisments { get; set; }
        public virtual ICollection<Qrcodesusers> Qrcodesusers { get; set; }
        public virtual ICollection<Redeemedcodes> Redeemedcodes { get; set; }
    }
}
