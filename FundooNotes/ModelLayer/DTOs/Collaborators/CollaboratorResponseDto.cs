namespace ModelLayer.DTOs.Collaborators
{
    public class CollaboratorResponseDto
    {
        public int CollaboratorId { get; set; }
        public string Email { get; set; } = null!;
        public string Permission { get; set; } = null!;
        public DateTime AddedAt { get; set; }
    }
}