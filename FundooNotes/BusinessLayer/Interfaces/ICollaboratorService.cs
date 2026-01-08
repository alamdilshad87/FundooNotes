using ModelLayer.DTOs.Collaborators;
using DataBaseLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorService
    {
        Task AddCollaboratorAsync(AddCollaboratorDto dto, int ownerId);
        Task<List<Collaborator>> GetCollaboratorsByNoteAsync(int noteId, int userId);
        Task RemoveCollaboratorAsync(int collaboratorId, int ownerId);
        Task UpdatePermissionAsync(int collaboratorId, string permission, int ownerId);
    }
}