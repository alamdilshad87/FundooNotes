using DataBaseLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ILabelService
    {
        Task<IEnumerable<Label>> GetLabelsAsync(int userId);
        Task CreateLabelAsync(string name, int userId);
        Task UpdateLabelAsync(int labelId, string name, int userId);
        Task DeleteLabelAsync(int labelId, int userId);

        // ✅ ADD THIS METHOD
        Task<IEnumerable<object>> GetNotesByLabelAsync(int labelId, int userId);
    }
}
