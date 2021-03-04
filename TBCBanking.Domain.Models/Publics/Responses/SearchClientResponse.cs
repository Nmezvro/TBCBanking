using System;
using System.Collections.Generic;
using System.Text;
using TBCBanking.Domain.Models.Publics.Common;

namespace TBCBanking.Domain.Models.Publics.Responses
{
    public class SearchClientResponse
    {
        public IEnumerable<SearchedClient> Clients { get; set; }
    }

    public class SearchedClient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
    }
}
