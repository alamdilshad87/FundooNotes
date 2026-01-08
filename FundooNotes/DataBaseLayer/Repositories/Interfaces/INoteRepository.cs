using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task AddAsync(Note note);
        Task<List<Note>> GetAllByUserAsync(int userId);
        Task<Note?> GetByIdAsync(int noteId, int userId);
        Task UpdateAsync(Note note);
        Task SaveAsync();
        Task<List<Note>> SearchAsync(int userId, string query);
        Task<List<Note>> GetByIdsAsync(List<int> noteIds, int userId);



    }
}