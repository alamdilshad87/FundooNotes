using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class NoteLabelService : INoteLabelService
    {
        private readonly INoteRepository _noteRepository;
        private readonly ILabelRepository _labelRepository;
        private readonly INoteLabelRepository _noteLabelRepository;

        public NoteLabelService(
            INoteRepository noteRepository,
            ILabelRepository labelRepository,
            INoteLabelRepository noteLabelRepository)
        {
            _noteRepository = noteRepository;
            _labelRepository = labelRepository;
            _noteLabelRepository = noteLabelRepository;
        }

        public async Task AddLabelToNoteAsync(int noteId, int labelId, int userId)
        {

            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null)
                throw new UnauthorizedException("Only note owner can add labels");

            var label = await _labelRepository.GetByIdAsync(labelId, userId);
            if (label == null)
                throw new NotFoundException("Label not found");

            bool exists = await _noteLabelRepository.ExistsAsync(noteId, labelId);
            if (exists)
                throw new ValidationException("Label already added to note");

            var noteLabel = new NoteLabel
            {
                NoteId = noteId,
                LabelId = labelId
            };

            await _noteLabelRepository.AddAsync(noteLabel);
            await _noteLabelRepository.SaveAsync();
        }

        public async Task RemoveLabelFromNoteAsync(int noteId, int labelId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null)
                throw new UnauthorizedException("Only note owner can remove labels");

            var mapping = await _noteLabelRepository.GetAsync(noteId, labelId)
                ?? throw new NotFoundException("Label not attached to note");

            await _noteLabelRepository.RemoveAsync(mapping);
            await _noteLabelRepository.SaveAsync();
        }

        public async Task<List<Label>> GetLabelsByNoteAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null)
                throw new NotFoundException("Note not found");

            var mappings = await _noteLabelRepository.GetByNoteIdAsync(noteId);

            return mappings.Select(m => m.Label).ToList();
        }
    }
}