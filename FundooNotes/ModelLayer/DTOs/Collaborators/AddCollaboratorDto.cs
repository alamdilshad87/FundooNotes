using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Collaborators
{
    public class AddCollaboratorDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public int NoteId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Permission { get; set; } = "Read";
    }
}