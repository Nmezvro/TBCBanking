using System;

namespace TBCBanking.Domain.Models.Exceptions
{
    [Serializable]
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException()
        {

        }

        public ClientNotFoundException(int clientId) : base(clientId.ToString())
        {

        }

        public ClientNotFoundException(int clientId, Exception innerException) : base(clientId.ToString(), innerException)
        {

        }
    }
}