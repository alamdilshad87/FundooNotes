using System.ComponentModel.DataAnnotations;

namespace DataBaseLayer.Entities
{
    public class NoteHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int NoteId { get; set; }
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Color { get; set; } = "white";

        public bool IsPinned { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}