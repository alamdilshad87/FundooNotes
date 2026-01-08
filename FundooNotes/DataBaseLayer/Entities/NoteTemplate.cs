using System.ComponentModel.DataAnnotations;

namespace DataBaseLayer.Entities
{
    public class NoteTemplate
    {
        [Key]
        public int TemplateId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Color { get; set; } = "white";
        public bool IsActive { get; set; } = true;
    }
}