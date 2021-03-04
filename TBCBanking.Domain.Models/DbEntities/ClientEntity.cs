using System;

namespace TBCBanking.Domain.Models.DbEntities
{
    public partial class ClientEntity
    {
        public ClientEntity()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte SexId { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCity { get; set; }
        public string PhotoUrl { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}