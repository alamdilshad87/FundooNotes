using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DataBaseLayer.Entities;

namespace DataBaseLayer.DbContexts
{
    public class FundooNotesdbContext : DbContext
    {
        public FundooNotesdbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }

    }
}
