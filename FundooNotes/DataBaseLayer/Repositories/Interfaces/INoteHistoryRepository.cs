using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface INoteHistoryRepository
    {
        Task AddAsync(NoteHistory history);
        Task<List<NoteHistory>> GetByNoteIdAsync(int noteId, int userId);
        Task SaveAsync();
    }
}