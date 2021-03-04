using Microsoft.EntityFrameworkCore;
using TBCBanking.Domain.Models.DbEntities;

namespace TBCBanking.Infrastructure.Repositories.DbEntities
{
    public partial class MainDBContext : DbContext
    {
        public MainDBContext()
        {
        }

        public MainDBContext(DbContextOptions<MainDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CityEntity> City { get; set; }
        public virtual DbSet<ClientEntity> Client { get; set; }
        public virtual DbSet<ClientPhoneNumberEntity> ClientPhoneNumber { get; set; }
        public virtual DbSet<ClientRelationEntity> ClientRelation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityEntity>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.NameEng)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.NameGeo)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<ClientEntity>(entity =>
            {
                entity.Property(e => e.BirthCity)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PersonalNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PhotoUrl).HasMaxLength(256);
            });

            modelBuilder.Entity<ClientPhoneNumberEntity>(entity =>
            {
                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
