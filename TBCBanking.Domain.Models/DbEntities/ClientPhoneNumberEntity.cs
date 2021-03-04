namespace TBCBanking.Domain.Models.DbEntities
{
    public partial class ClientPhoneNumberEntity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public byte TypeId { get; set; }
        public string Phone { get; set; }
    }
}