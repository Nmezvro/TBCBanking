using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.DbEntities;
using TBCBanking.Domain.Models.DbEntities.Custom;
using TBCBanking.Domain.Models.DbEntities.ProcedureArgs;
using TBCBanking.Domain.Models.Exceptions;
using TBCBanking.Domain.Models.Publics.Requests;
using TBCBanking.Domain.Repositories;
using TBCBanking.Infrastructure.Extensions;
using TBCBanking.Infrastructure.Repositories.DbEntities;
using Z.EntityFramework.Plus;

namespace TBCBanking.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository, IDisposable
    {
        private bool disposed = false;
        private readonly MainDBContext _db;

        public ClientRepository(MainDBContext db)
        {
            _db = db;
        }

        public async Task<bool> CityIsNotValid(string id)
        {
            CityEntity city = await _db.City.FindAsync(id);
            return city == null;
        }

        public async Task<bool> ClientDoesNotExist(int id)
        {
            ClientEntity client = await _db.Client.FindAsync(id);
            return client == null;
        }

        public async Task<int> RegisterClient(RegisterClientRequest request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _db.Database.BeginTransaction();
            ClientEntity client = new ClientEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                SexId = (byte)request.Sex,
                PersonalNumber = request.PersonalNumber,
                BirthDate = request.BirthDate,
                BirthCity = request.City,
                //PhotoUrl = request.PhotoAddress,
                StatusId = 1
            };
            await _db.Client.SingleInsertAsync(client);
            await _db.ClientPhoneNumber.BulkInsertAsync(request.PhoneNumbers.Select(x => new ClientPhoneNumberEntity { ClientId = client.Id, TypeId = (byte)x.Type, Phone = x.Phone }));
            //await _db.ClientRelation.BulkInsertAsync(request.RelatedClients.Select(x => new ClientRelation { ClientId = client.Id, TypeId = (byte)x.Type, RelativeId = x.Id }));
            await transaction.CommitAsync();
            return client.Id;
        }

        public async Task<int> UpdateClient(UpdateClientRequest request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _db.Database.BeginTransaction();
            ClientEntity client = await _db.Client.FindAsync(request.Id);
            if (client == null) throw new ClientNotFoundException(request.Id);
            await _db.Client.Where(a => a.Id == request.Id).UpdateAsync(c => new ClientEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                SexId = (byte)request.Sex,
                PersonalNumber = request.PersonalNumber,
                BirthDate = request.BirthDate,
                BirthCity = request.City,
                PhotoUrl = request.PhotoAddress,
            });
            if (request.PhoneNumbers.Any())
                await _db.ClientPhoneNumber.Where(c => c.ClientId == client.Id && !request.PhoneNumbers.Select(p => (byte)p.Type).Contains(c.TypeId)).DeleteAsync();
            await _db.BulkMergeAsync(request.PhoneNumbers.Select(x => new ClientPhoneNumberEntity { ClientId = request.Id, TypeId = (byte)x.Type, Phone = x.Phone }), o => o.ColumnPrimaryKeyExpression = c => new { c.ClientId, c.TypeId });
            await transaction.CommitAsync();
            return client.Id;
        }

        public async Task DeactivateClient(int clientId)
        {
            await _db.Client.Where(a => a.Id == clientId).UpdateAsync(c => new ClientEntity
            {
                StatusId = 0
            });
        }

        public async Task<FullClientEntity> GetClient(int clientId)
        {
            var dbEntity = new FullClientEntity();
            dbEntity.Client = await _db.Client.FindAsync(clientId);
            //ASYNC??? რა დებილობაა ეს EF
            dbEntity.Phones = _db.ClientPhoneNumber.Where(a => a.ClientId == clientId).AsEnumerable();
            dbEntity.Relatives = _db.ClientRelation.Where(a => a.ClientId == clientId).AsEnumerable();
            return dbEntity;
        }

        public async Task<IEnumerable<ClientEntity>> QuickSearchClient(QuickClientSearchRequest request)
        {
            return await _db.Client.Where(a =>
            (!string.IsNullOrEmpty(request.PersonalNumber) || a.PersonalNumber == request.PersonalNumber) &&
            (!string.IsNullOrEmpty(request.FirstName) || a.FirstName.Contains(request.FirstName)) &&
            (!string.IsNullOrEmpty(request.LastName) || a.LastName.Contains(request.LastName)) &&
            (!request.BirthDate.HasValue || a.BirthDate == request.BirthDate)).ToListAsync();
        }

        public async Task<IEnumerable<SearchedClientEntity>> SearchClient(ClientSearchRequest request)
        {
            using (SqlConnection dbConn = (SqlConnection)_db.Database.GetDbConnection())
            {
                ClientSearchArgs args = new ClientSearchArgs(request.Page, request.PageSize);
                args.FirstName = request.FirstName;
                args.LastName = request.LastName;
                if (request.Sex.HasValue) args.SexId = (byte)request.Sex.Value;
                args.PersonalNumber = request.PersonalNumber;
                args.BirthDate = request.BirthDate;
                args.BirthCity = request.City;
                args.PhoneNumber = request.PhoneNumber;
                return await dbConn.ProcedureReader<SearchedClientEntity, ClientSearchArgs>("uspClientSearch", args);
            }
        }

        public async Task PutClientPhoto(int clientId, string photoPath)
        {
            await _db.Client.Where(a => a.Id == clientId).UpdateAsync(c => new ClientEntity
            {
                PhotoUrl = photoPath
            });
        }

        public async Task AddClientRelatives(AddClientRelativesRequest request)
        {
            await _db.BulkMergeAsync(request.RelatedClients.Select(x => new ClientRelationEntity { ClientId = request.ClientId, TypeId = (byte)x.Type, RelativeId = x.Id }), o => o.ColumnPrimaryKeyExpression = c => new { c.ClientId, c.TypeId });
        }

        public async Task DeleteClientRelatives(DeleteClientRelativesRequest request)
        {
            await _db.ClientRelation.Where(a => a.ClientId == request.ClientId && request.RelatedClients.Contains(a.RelativeId)).DeleteAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
                _db.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}