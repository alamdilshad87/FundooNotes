using System.ComponentModel.DataAnnotations;

namespace DataBaseLayer.Entities
{
    public class Collaborator
    {
        [Key]
        public int CollaboratorId { get; set; }

        public int NoteId { get; set; }
        public int UserId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Permission { get; set; } = "Read";

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public Note Note { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}