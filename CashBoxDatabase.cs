﻿using Microsoft.EntityFrameworkCore;

namespace CHRBerserk.BerserksCashbox
{
    public class CashBoxDatabase : DbContext
    {
        public DbSet<CashBox> CashBoxOperations { get; set; }

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
