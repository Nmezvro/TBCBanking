using System;
using System.Collections.Generic;
using TBCBanking.Domain.Models.Publics.Common;

namespace TBCBanking.Domain.Models.Publics.Requests
{
    public class RegisterClientRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public IEnumerable<ClientPhoneNumber> PhoneNumbers { get; set; }
        //public string PhotoAddress { get; set; }
        //public IEnumerable<RelatedClientRequest> RelatedClients { get; set; }
    }
}