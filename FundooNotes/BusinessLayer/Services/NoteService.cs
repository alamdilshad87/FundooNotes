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

        public NoteService(INoteRepository noteRepository, INoteTemplateRepository templateRepository, INoteHistoryRepository noteHistoryRepository)
        {
            _noteRepository = noteRepository;
            _templateRepository = templateRepository;
            _historyRepository = noteHistoryRepository;
        }

        public async Task CreateNoteAsync(CreateNoteDto dto, int userId)
        {
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                Color = dto.Color,
                UserId = userId
            };

            await _noteRepository.AddAsync(note);
            await _noteRepository.SaveAsync();
        }
        public async Task<List<Note>> GetAllNotesAsync(int userId)
        {
            return await _noteRepository.GetAllByUserAsync(userId);
        }
        public async Task<Note> GetNoteByIdAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

            return note;
        }
        public async Task UpdateNoteAsync(int noteId, UpdateNoteDto dto, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

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
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.IsDeleted = true;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }
        public async Task TogglePinAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.IsPinned = !note.IsPinned;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }
        public async Task ToggleArchiveAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.IsArchived = !note.IsArchived;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }
        public async Task UpdateNoteColorAsync(int noteId, UpdateNoteColorDto dto, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

            await SaveHistory(note);

            note.Color = dto.Color;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            await _noteRepository.SaveAsync();
        }
        public async Task<List<Note>> SearchNotesAsync(int userId, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Note>();

            return await _noteRepository.SearchAsync(userId, query);
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
            var template = await _templateRepository.GetByIdAsync(templateId);

            if (template == null)
                throw new NotFoundException("Template not found");

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
        public async Task<List<NoteHistory>> GetNoteHistoryAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);

            if (note == null)
                throw new NotFoundException("Note not found");

            return await _historyRepository.GetByNoteIdAsync(noteId, userId);
        }
    }
}