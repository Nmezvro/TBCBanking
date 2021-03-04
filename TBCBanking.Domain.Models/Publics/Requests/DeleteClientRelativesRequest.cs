using System.Collections.Generic;

namespace TBCBanking.Domain.Models.Publics.Requests
{
    public class DeleteClientRelativesRequest
    {
        public int ClientId { get; set; }
        public IEnumerable<int> RelatedClients { get; set; }
    }
}