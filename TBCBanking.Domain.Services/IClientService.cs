using System.Threading.Tasks;
using TBCBanking.Domain.Models.Publics.Requests;
using TBCBanking.Domain.Models.Publics.Responses;

namespace TBCBanking.Domain.Services
{
    public interface IClientService
    {
        Task<int> RegisterClient(RegisterClientRequest request);
        Task<int> UpdateClient(UpdateClientRequest request);
        Task DeactivateClient(DeactivateClientRequest request);
        Task<ClientResponse> GetClient(int clientId);
        Task<SearchClientResponse> QuickSearchClient(QuickClientSearchRequest request);
        Task<SearchClientResponse> SearchClient(ClientSearchRequest request);
        Task PutClientPhoto(PutClientPhotoRequest request);
        Task AddClientRelatives(AddClientRelativesRequest request);
        Task DeleteClientRelatives(DeleteClientRelativesRequest request);
    }
}