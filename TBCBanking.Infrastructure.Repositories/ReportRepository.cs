using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.DbEntities;
using TBCBanking.Domain.Repositories;
using TBCBanking.Infrastructure.Extensions;
using TBCBanking.Infrastructure.Repositories.DbEntities;

namespace TBCBanking.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository, IDisposable
    {
        private bool disposed = false;
        private readonly MainDBContext _db;

        public ReportRepository(MainDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Report1Entity>> Report1()
        {
            using (SqlConnection dbConn = (SqlConnection)_db.Database.GetDbConnection())
            {
                return await dbConn.ProcedureReader<Report1Entity>("uspReport1");
            }
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