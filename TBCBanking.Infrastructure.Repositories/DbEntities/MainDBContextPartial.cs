using Microsoft.EntityFrameworkCore;

namespace TBCBanking.Infrastructure.Repositories.DbEntities
{
    public partial class MainDBContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

        }
    }
}