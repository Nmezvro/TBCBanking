using System.Collections.Generic;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.DbEntities;
using TBCBanking.Domain.Models.DbEntities.Custom;
using TBCBanking.Domain.Models.Publics.Requests;

namespace TBCBanking.Domain.Repositories
{
    public interface IClientRepository
    {
        Task<bool> CityIsNotValid(string value);
        Task<bool> ClientDoesNotExist(int id);
        Task<int> RegisterClient(RegisterClientRequest request);
        Task<int> UpdateClient(UpdateClientRequest request);
        Task DeactivateClient(int clientId);
        Task<FullClientEntity> GetClient(int clientId);
        Task<IEnumerable<ClientEntity>> QuickSearchClient(QuickClientSearchRequest request);
        Task<IEnumerable<SearchedClientEntity>> SearchClient(ClientSearchRequest request);
        Task PutClientPhoto(int clientId, string photoPath);
        Task AddClientRelatives(AddClientRelativesRequest request);
        Task DeleteClientRelatives(DeleteClientRelativesRequest request);
    }
}