using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Notes;
using ModelLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly INoteTemplateRepository _templateRepository;
        private readonly INoteHistoryRepository _historyRepository;

        public NoteService(
            INoteRepository noteRepository,
            INoteTemplateRepository templateRepository,
            INoteHistoryRepository historyRepository)
        {
            _noteRepository = noteRepository;
            _templateRepository = templateRepository;
            _historyRepository = historyRepository;
        }

        public async Task CreateNoteAsync(CreateNoteDto dto, int userId)
        {
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                Color = dto.Color,
                IsArchived = dto.IsArchived,
                IsPinned = dto.IsPinned, // ✅ This should be here
                UserId = userId,
                CreatedAt = DateTime.UtcNow, // ✅ Explicitly set
                UpdatedAt = DateTime.UtcNow  // ✅ Explicitly set
            };

            await _noteRepository.AddAsync(note);
            await _noteRepository.SaveAsync();
        }




        public async Task<List<NoteResponseDto>> GetAllNotesAsync(int userId)
        {
            var notes = await _noteRepository.GetAllByUserAsync(userId);
            return notes.Select(MapToDto).ToList();
        }

        public async Task<NoteResponseDto> GetNoteByIdAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            return MapToDto(note);
        }

        public async Task UpdateNoteAsync(int noteId, UpdateNoteDto dto, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.Color = dto.Color;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task DeleteNoteAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.IsDeleted = true;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task<List<NoteResponseDto>> GetTrashedNotesAsync(int userId)
        {
            var notes = await _noteRepository.GetAllByUserAsync(userId);
            var trashedNotes = notes.Where(n => n.IsDeleted).ToList();
            return trashedNotes.Select(MapToDto).ToList();
        }

        public async Task<List<NoteResponseDto>> GetArchivedNotesAsync(int userId)
        {
            var notes = await _noteRepository.GetAllByUserAsync(userId);
            var archivedNotes = notes.Where(n => n.IsArchived && !n.IsDeleted).ToList();
            return archivedNotes.Select(MapToDto).ToList();
        }

        public async Task RestoreNoteAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            if (!note.IsDeleted)
                throw new InvalidOperationException("Note is not in trash");

            await SaveHistory(note);

            note.IsDeleted = false;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task PermanentDeleteNoteAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            await SaveHistory(note);

            await _noteRepository.DeleteAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task TogglePinAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.IsPinned = !note.IsPinned;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task ToggleArchiveAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.IsArchived = !note.IsArchived;
            note.IsPinned = false; 
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task UpdateNoteColorAsync(int noteId, UpdateNoteColorDto dto, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.Color = dto.Color;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task<List<NoteResponseDto>> SearchNotesAsync(int userId, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<NoteResponseDto>();

            var notes = await _noteRepository.SearchAsync(userId, query);
            return notes.Select(MapToDto).ToList();
        }

        public async Task BulkDeleteAsync(List<int> noteIds, int userId)
        {
            var notes = await _noteRepository.GetByIdsAsync(noteIds, userId);

            foreach (var note in notes)
            {
                await SaveHistory(note);
                note.IsDeleted = true;
                note.UpdatedAt = DateTime.UtcNow;
            }

            await _noteRepository.SaveAsync();
        }

        public async Task CreateFromTemplateAsync(int templateId, int userId)
        {
            var template = await _templateRepository.GetByIdAsync(templateId)
                ?? throw new NotFoundException("Template not found");

            var note = new Note
            {
                Title = template.Title,
                Content = template.Content,
                Color = template.Color,
                UserId = userId
            };

            await _noteRepository.AddAsync(note);
            await _noteRepository.SaveAsync();
        }

        public async Task<List<NoteHistory>> GetNoteHistoryAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId)
                ?? throw new NotFoundException("Note not found");

            return await _historyRepository.GetByNoteIdAsync(noteId, userId);
        }

        private async Task SaveHistory(Note note)
        {
            var history = new NoteHistory
            {
                NoteId = note.NoteId,
                UserId = note.UserId,
                Title = note.Title,
                Content = note.Content,
                Color = note.Color,
                IsPinned = note.IsPinned,
                IsArchived = note.IsArchived,
                IsDeleted = note.IsDeleted,
                ChangedAt = DateTime.UtcNow
            };

            await _historyRepository.AddAsync(history);
            await _historyRepository.SaveAsync();
        }

        // Map Note entity to DTO
        private static NoteResponseDto MapToDto(Note note)
        {
            return new NoteResponseDto
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Content = note.Content,
                IsPinned = note.IsPinned,
                IsArchived = note.IsArchived,
                IsDeleted = note.IsDeleted,
                Color = note.Color,
                Labels = note.NoteLabels
                    .Select(nl => nl.Label.Name)
                    .ToList()
            };
        }
    }
}
