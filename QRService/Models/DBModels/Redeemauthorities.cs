namespace QRService.Models.DBModels
{
    public partial class Redeemauthorities
    {
        public int? AuthorityId { get; set; }
        public int? InstitutionId { get; set; }
        public string Pin { get; set; }
        public int RedeemAuthoritiesId { get; set; }
    }
}
