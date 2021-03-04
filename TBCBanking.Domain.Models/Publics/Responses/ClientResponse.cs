using System;
using System.Collections.Generic;
using TBCBanking.Domain.Models.Publics.Common;

namespace TBCBanking.Domain.Models.Publics.Responses
{
    public class ClientResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCity { get; set; }
        public string PhotoUrl { get; set; }
        public ClientStatus Status { get; set; }
        public DateTime CreateDate { get; set; }

        public IEnumerable<ClientPhoneNumber> PhoneNumbers { get; set; }
        public IEnumerable<RelatedClient> Relatives { get; set; }
    }
}