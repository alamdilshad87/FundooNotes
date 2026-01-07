using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Notes
{
    public class CreateNoteDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Color { get; set; } = "white";
    }
}