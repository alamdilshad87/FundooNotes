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
        Task TogglePinAsync(int noteId, int userId);
        Task ToggleArchiveAsync(int noteId, int userId);
        Task UpdateNoteColorAsync(int noteId, UpdateNoteColorDto dto, int userId);
        Task<List<Note>> SearchNotesAsync(int userId, string query);

    }
}