using ModelLayer.DTOs.Notes;

namespace BusinessLayer.Interfaces
{
    public interface INoteService
    {
        Task CreateNoteAsync(CreateNoteDto dto, int userId);
    }
}