using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Notes
{
    public class CreateNoteDto
    {
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Color { get; set; } = "white";

        public bool IsArchived { get; set; } = false; // ✅ ADD THIS
    }
}
