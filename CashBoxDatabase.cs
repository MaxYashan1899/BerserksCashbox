using Microsoft.EntityFrameworkCore;

namespace BerserksCashbox
{
    public class CashBoxDatabase : DbContext
    {
        public DbSet<CashBoxOperation> CashBoxOperations { get; set; }

        public CashBoxDatabase()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CashBoxappdb;Trusted_Connection=True;");
        }

    }
}
