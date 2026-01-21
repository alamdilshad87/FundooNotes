using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<Note?> GetByIdAsync(int noteId, int userId);
        Task<List<Note>> GetAllByUserAsync(int userId);
        Task<List<Note>> GetByIdsAsync(List<int> noteIds, int userId);
        Task<List<Note>> SearchAsync(int userId, string query);

        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Note note); // ✅ ADD THIS
        Task SaveAsync();
    }
}
