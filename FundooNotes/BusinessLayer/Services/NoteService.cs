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

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
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
    }
}