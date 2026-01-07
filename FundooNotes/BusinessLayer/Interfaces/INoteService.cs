using DataBaseLayer.Entities;
using ModelLayer.DTOs.Notes;

namespace BusinessLayer.Interfaces
{
    public interface INoteService
    {
        Task CreateNoteAsync(CreateNoteDto dto, int userId);
        Task<List<Note>> GetAllNotesAsync(int userId);
        Task<Note> GetNoteByIdAsync(int noteId, int userId);
        Task UpdateNoteAsync(int noteId, UpdateNoteDto dto, int userId);
        Task DeleteNoteAsync(int noteId, int userId);

    }
}