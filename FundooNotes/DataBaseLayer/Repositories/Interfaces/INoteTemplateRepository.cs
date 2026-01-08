using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface INoteTemplateRepository
    {
        Task<NoteTemplate?> GetByIdAsync(int templateId);
    }
}