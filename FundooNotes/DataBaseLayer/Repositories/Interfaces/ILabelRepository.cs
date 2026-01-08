using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface ILabelRepository
    {
        Task AddAsync(Label label);
        Task<List<Label>> GetAllAsync(int userId);
        Task<Label?> GetByIdAsync(int labelId, int userId);
        Task SaveAsync();
    }
}