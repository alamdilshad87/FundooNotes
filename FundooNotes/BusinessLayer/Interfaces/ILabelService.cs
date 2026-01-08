using DataBaseLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ILabelService
    {
        Task CreateLabelAsync(string name, int userId);
        Task<List<Label>> GetLabelsAsync(int userId);
        Task UpdateLabelAsync(int labelId, string name, int userId);
        Task DeleteLabelAsync(int labelId, int userId);
    }
}