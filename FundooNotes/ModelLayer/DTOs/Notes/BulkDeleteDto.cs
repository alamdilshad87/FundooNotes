using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Notes
{
    public class BulkDeleteDto
    {
        [Required]
        public List<int> NoteIds { get; set; } = new();
    }
}