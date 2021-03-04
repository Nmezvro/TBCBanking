using System.Collections.Generic;
using TBCBanking.Domain.Models.Publics.Common;

namespace TBCBanking.Domain.Models.Publics.Requests
{
    public class AddClientRelativesRequest
    {
        public int ClientId { get; set; }
        public IEnumerable<RelatedClient> RelatedClients { get; set; }
    }
}