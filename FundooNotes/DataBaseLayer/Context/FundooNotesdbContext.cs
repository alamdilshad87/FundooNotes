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
        public DbSet<Label> Labels { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<NoteLabel> NoteLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ ADD THIS - Configure Note entity explicitly
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Content)
                    .IsRequired(false);

                entity.Property(e => e.IsPinned)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.IsArchived)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(20)
                    .HasDefaultValue("white");

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Collaborator>()
                .HasOne(c => c.Note)
                .WithMany(n => n.Collaborators)
                .HasForeignKey(c => c.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Collaborator>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NoteId);

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Label)
                .WithMany()
                .HasForeignKey(nl => nl.LabelId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
