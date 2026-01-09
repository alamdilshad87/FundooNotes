using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface INoteLabelRepository
    {
        Task AddAsync(NoteLabel noteLabel);
        Task<bool> ExistsAsync(int noteId, int labelId);
        Task<NoteLabel?> GetAsync(int noteId, int labelId);
        Task<List<NoteLabel>> GetByNoteIdAsync(int noteId);
        Task RemoveAsync(NoteLabel noteLabel);
        Task SaveAsync();
    }
}