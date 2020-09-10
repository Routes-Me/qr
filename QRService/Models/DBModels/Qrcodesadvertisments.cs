namespace QRService.Models.DBModels
{
    public partial class Qrcodesadvertisments
    {
        public int QrCodesAdvertismentsId { get; set; }
        public int? AdvertismentId { get; set; }
        public int? QrCodeId { get; set; }

        public virtual Qrcodes QrCode { get; set; }
    }
}
