using System;
using TBCBanking.Domain.Models.DbExtensions;

namespace TBCBanking.Domain.Models.DbEntities.ProcedureArgs
{
    public class ClientSearchArgs
    {
        public ClientSearchArgs(int page, int pageSize)
        {
            Page = page; PageSize = pageSize;
        }

        [ProcedureParameter("FirstName")] public string FirstName { get; set; }
        [ProcedureParameter("LastName")] public string LastName { get; set; }
        [ProcedureParameter("SexId")] public byte? SexId { get; set; }
        [ProcedureParameter("PersonalNumber")] public string PersonalNumber { get; set; }
        [ProcedureParameter("BirthDate")] public DateTime? BirthDate { get; set; }
        [ProcedureParameter("BirthCity")] public string BirthCity { get; set; }
        [ProcedureParameter("PhoneNumber")] public string PhoneNumber { get; set; }
        [ProcedureParameter("Page ")] public int Page { get; set; }
        [ProcedureParameter("PageSize ")] public int PageSize { get; set; }
    }
}