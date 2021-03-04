using System.Collections.Generic;

namespace TBCBanking.Domain.Models.DbEntities.Custom
{
    public class FullClientEntity
    {
        public ClientEntity Client { get; set; }
        public IEnumerable<ClientPhoneNumberEntity> Phones { get; set; }
        public IEnumerable<ClientRelationEntity> Relatives { get; set; }
    }
}