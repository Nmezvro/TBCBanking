using System;
using TBCBanking.Domain.Models.Publics.Common;

namespace TBCBanking.Domain.Models.Publics.Requests
{
    public class QuickClientSearchRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    public class ClientSearchRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex? Sex { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}