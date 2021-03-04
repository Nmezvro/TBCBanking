using System.Collections.Generic;
using TBCBanking.Domain.Models.Publics.Common;

namespace TBCBanking.Domain.Models.Publics.Responses
{
    public class Report1Response
    {
        public IEnumerable<Report1> Data { get; set; }
    }

    public class Report1
    {
        public int ClientId { get; set; }
        public string PersonalNumber { get; set; }
        public IEnumerable<Report1Data> Data { get; set; }
    }

    public class Report1Data
    {
        public RelatedClientType Type { get; set; }
        public int RelativeCount { get; set; }
    }
}