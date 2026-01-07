using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Notes;

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
    }
}