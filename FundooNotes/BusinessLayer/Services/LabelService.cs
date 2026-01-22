using BusinessLayer.Interfaces;
using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class LabelService : ILabelService
    {
        private readonly FundooNotesDbContext _context;

        public LabelService(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Label>> GetLabelsAsync(int userId)
        {
            return await _context.Labels
                .Where(l => l.UserId == userId && !l.IsDeleted)
                .OrderBy(l => l.Name)
                .ToListAsync();
        }

        public async Task CreateLabelAsync(string name, int userId)
        {
            var label = new Label
            {
                Name = name,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Labels.Add(label);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLabelAsync(int labelId, string name, int userId)
        {
            var label = await _context.Labels
                .FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            label.Name = name;
            label.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLabelAsync(int labelId, int userId)
        {
            var label = await _context.Labels
                .FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            label.IsDeleted = true;
            label.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // ✅ ADD THIS NEW METHOD
        public async Task<IEnumerable<object>> GetNotesByLabelAsync(int labelId, int userId)
        {
            // Verify label belongs to user
            var label = await _context.Labels
                .FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId && !l.IsDeleted);

            if (label == null)
                throw new Exception("Label not found");

            // Get all notes with this label
            var notes = await _context.NoteLabels
                .Where(nl => nl.LabelId == labelId)
                .Include(nl => nl.Note)
                    .ThenInclude(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .Where(nl => nl.Note.UserId == userId && !nl.Note.IsDeleted)
                .Select(nl => nl.Note)
                .Distinct()
                .ToListAsync();

            // Format response
            var result = notes.Select(note => new
            {
                noteId = note.NoteId,
                title = note.Title,
                content = note.Content,
                color = note.Color,
                isPinned = note.IsPinned,
                isArchived = note.IsArchived,
                isTrashed = note.IsDeleted,
                createdAt = note.CreatedAt,
                updatedAt = note.UpdatedAt,
                labels = note.NoteLabels
                    .Where(nl => !nl.Label.IsDeleted)
                    .Select(nl => nl.Label.Name)
                    .ToList()
            }).ToList();

            return result;
        }
    }
}
