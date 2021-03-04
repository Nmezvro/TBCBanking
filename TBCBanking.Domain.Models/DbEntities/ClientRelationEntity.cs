namespace TBCBanking.Domain.Models.DbEntities
{
    public partial class ClientRelationEntity
    {
        public int Id { get; set; }
        public byte TypeId { get; set; }
        public int ClientId { get; set; }
        public int RelativeId { get; set; }
    }
}