using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.Configuration;
using TBCBanking.Domain.Models.Exceptions;
using TBCBanking.Domain.Models.Publics.Common;
using TBCBanking.Domain.Models.Publics.Requests;
using TBCBanking.Domain.Models.Publics.Responses;
using TBCBanking.Domain.Repositories;
using TBCBanking.Domain.Services;

namespace TBCBanking.Infrastructure.Services
{
    public class ClientService : IClientService
    {
        private readonly ApiDefaults _defaults;
        private readonly IClientRepository _repository;
        private readonly IFileStorageRepository _repositoryFiles;

        public ClientService(IOptions<ApiDefaults> defaults,
            IClientRepository repository, IFileStorageRepository repositoryFiles)
        {
            _defaults = defaults.Value;
            _repository = repository;
            _repositoryFiles = repositoryFiles;
        }

        public async Task<int> RegisterClient(RegisterClientRequest request)
        {
            return await _repository.RegisterClient(request);
        }

        public async Task<int> UpdateClient(UpdateClientRequest request)
        {
            return await _repository.UpdateClient(request);
        }

        public async Task DeactivateClient(DeactivateClientRequest request)
        {
            await _repository.DeactivateClient(request.ClientId);
        }

        public async Task<ClientResponse> GetClient(int clientId)
        {
            var dbEntity = await _repository.GetClient(clientId);
            var dbClient = dbEntity.Client;
            if (dbClient == null) throw new ClientNotFoundException(clientId);
            ClientResponse client = new ClientResponse();
            client.Id = dbClient.Id;
            client.FirstName = dbClient.FirstName;
            client.LastName = dbClient.LastName;
            client.Sex = (Sex)dbClient.SexId;
            client.PersonalNumber = dbClient.PersonalNumber;
            client.BirthDate = dbClient.BirthDate;
            client.BirthCity = dbClient.BirthCity;
            client.PhotoUrl = dbClient.PhotoUrl;
            client.Status = (ClientStatus)dbClient.StatusId;
            client.CreateDate = dbClient.CreateDate;
            client.PhoneNumbers = dbEntity.Phones.Select(a => new ClientPhoneNumber() { Type = (ClientPhoneNumberType)a.TypeId, Phone = a.Phone });
            client.Relatives = dbEntity.Relatives.Select(a => new RelatedClient() { Type = (RelatedClientType)a.TypeId, Id = a.RelativeId });
            return client;
        }

        public async Task<SearchClientResponse> QuickSearchClient(QuickClientSearchRequest request)
        {
            System.Collections.Generic.IEnumerable<Domain.Models.DbEntities.ClientEntity> dbEntity = await _repository.QuickSearchClient(request);

            return new SearchClientResponse
            {
                Clients = dbEntity.Select(a =>
                new SearchedClient
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    BirthDate = a.BirthDate,
                    City = a.BirthCity,
                    PersonalNumber = a.PersonalNumber,
                    Sex = (Sex)a.SexId
                })
            };
        }

        public async Task<SearchClientResponse> SearchClient(ClientSearchRequest request)
        {
            System.Collections.Generic.IEnumerable<Domain.Models.DbEntities.SearchedClientEntity> dbEntity = await _repository.SearchClient(request);

            return new SearchClientResponse
            {
                Clients = dbEntity.Select(a =>
                new SearchedClient
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    BirthDate = a.BirthDate,
                    City = a.BirthCity,
                    PersonalNumber = a.PersonalNumber,
                    Sex = (Sex)a.SexId
                })
            };
        }

        public async Task PutClientPhoto(PutClientPhotoRequest request)
        {
            //request.PhotoData = await File.ReadAllBytesAsync(_defaults.FileStorageDirectory + "nika.png");
            string filePath = Path.Combine(_defaults.FileStorageDirectory, Guid.NewGuid().ToString() + "." + request.PhotoName);
            await _repositoryFiles.SaveFile(request.PhotoData, filePath);
            await _repository.PutClientPhoto(request.ClientId, filePath);
        }

        public async Task AddClientRelatives(AddClientRelativesRequest request)
        {
            await _repository.AddClientRelatives(request);
        }

        public async Task DeleteClientRelatives(DeleteClientRelativesRequest request)
        {
            await _repository.DeleteClientRelatives(request);
        }
    }
}