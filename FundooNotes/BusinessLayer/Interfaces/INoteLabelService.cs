using DataBaseLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface INoteLabelService
    {
        Task AddLabelToNoteAsync(int noteId, int labelId, int userId);
        Task RemoveLabelFromNoteAsync(int noteId, int labelId, int userId);
        Task<List<Label>> GetLabelsByNoteAsync(int noteId, int userId);
    }
}