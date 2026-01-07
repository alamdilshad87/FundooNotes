using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Notes
{
    public class UpdateNoteColorDto
    {
        [Required]
        [MaxLength(20)]
        public string Color { get; set; } = "white";
    }
}