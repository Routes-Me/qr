namespace QRService.Models.DBModels
{
    public partial class Qrcodesusers
    {
        public int QrCodesUsersId { get; set; }
        public int? UserId { get; set; }
        public int? QrCodeId { get; set; }

        public virtual Qrcodes QrCode { get; set; }
    }
}
