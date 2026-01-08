using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Collaborators
{
    public class UpdatePermissionDto
    {
        [Required]
        [MaxLength(10)]
        public string Permission { get; set; } = null!;
    }
}