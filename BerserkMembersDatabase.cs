using Microsoft.EntityFrameworkCore;

namespace CHRBerserk.BerserksCashbox
{
    public class BerserkMembersDatabase : DbContext
    {
        public DbSet<BerserkMembers> BerserkMembers { get; set; }
    

        public BerserkMembersDatabase()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BerserkMembersappdb;Trusted_Connection=True;");
        }
    }
}
