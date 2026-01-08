using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface ICollaboratorRepository
    {
        Task AddAsync(Collaborator collaborator);
        Task<bool> ExistsAsync(int noteId, int userId);
        Task<List<Collaborator>> GetByNoteIdAsync(int noteId);
        Task<Collaborator?> GetByIdAsync(int collaboratorId);
        Task RemoveAsync(Collaborator collaborator);
        Task SaveAsync();
    }
}