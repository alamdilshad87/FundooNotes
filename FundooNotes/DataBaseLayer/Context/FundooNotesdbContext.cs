using Microsoft.EntityFrameworkCore;
using DataBaseLayer.Entities;

namespace DataBaseLayer.Context
{
    public class FundooNotesDbContext : DbContext
    {
        public FundooNotesDbContext(DbContextOptions<FundooNotesDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteTemplate> NoteTemplates { get; set; }
        public DbSet<NoteHistory> NoteHistories { get; set; }

    }
}