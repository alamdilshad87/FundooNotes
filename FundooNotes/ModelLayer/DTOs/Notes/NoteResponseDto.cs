namespace ModelLayer.DTOs.Notes
{
    public class NoteResponseDto
    {
        public int NoteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPinned { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public string Color { get; set; } = "white";

        public List<string> Labels { get; set; } = new();
    }
}
