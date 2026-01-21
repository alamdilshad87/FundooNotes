using DataBaseLayer.Entities;
using ModelLayer.DTOs.Notes;

namespace BusinessLayer.Interfaces
{
    public interface INoteService
    {
        Task CreateNoteAsync(CreateNoteDto dto, int userId); // ✅ Changed back to Task
        Task<List<NoteResponseDto>> GetAllNotesAsync(int userId);
        Task<NoteResponseDto> GetNoteByIdAsync(int noteId, int userId);
        Task UpdateNoteAsync(int noteId, UpdateNoteDto dto, int userId);
        Task DeleteNoteAsync(int noteId, int userId);
        Task<List<NoteResponseDto>> GetTrashedNotesAsync(int userId);
        Task<List<NoteResponseDto>> GetArchivedNotesAsync(int userId);
        Task RestoreNoteAsync(int noteId, int userId);
        Task PermanentDeleteNoteAsync(int noteId, int userId);
        Task TogglePinAsync(int noteId, int userId);
        Task ToggleArchiveAsync(int noteId, int userId);
        Task UpdateNoteColorAsync(int noteId, UpdateNoteColorDto dto, int userId);
        Task<List<NoteResponseDto>> SearchNotesAsync(int userId, string query);
        Task BulkDeleteAsync(List<int> noteIds, int userId);
        Task CreateFromTemplateAsync(int templateId, int userId);
        Task<List<NoteHistory>> GetNoteHistoryAsync(int noteId, int userId);
    }
}
