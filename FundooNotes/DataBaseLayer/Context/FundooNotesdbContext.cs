using System;
using Microsoft.EntityFrameworkCore;
using DataBaseLayer.Entities;

namespace DataBaseLayer.Context
{
    public class FundooNotesDbContext : DbContext
    {
        public FundooNotesDbContext(DbContextOptions<FundooNotesDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
