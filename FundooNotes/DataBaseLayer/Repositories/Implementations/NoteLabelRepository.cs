using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBaseLayer.Repositories.Implementations
{
    public class NoteLabelRepository : INoteLabelRepository
    {
        private readonly FundooNotesDbContext _context;

        public NoteLabelRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NoteLabel noteLabel)
        {
            await _context.NoteLabels.AddAsync(noteLabel);
        }

        public async Task<bool> ExistsAsync(int noteId, int labelId)
        {
            return await _context.NoteLabels
                .AnyAsync(nl => nl.NoteId == noteId && nl.LabelId == labelId);
        }

        public async Task<NoteLabel?> GetAsync(int noteId, int labelId)
        {
            return await _context.NoteLabels
                .FirstOrDefaultAsync(nl => nl.NoteId == noteId && nl.LabelId == labelId);
        }

        public async Task<List<NoteLabel>> GetByNoteIdAsync(int noteId)
        {
            return await _context.NoteLabels
                .Where(nl => nl.NoteId == noteId)
                .Include(nl => nl.Label)
                .ToListAsync();
        }

        public async Task RemoveAsync(NoteLabel noteLabel)
        {
            _context.NoteLabels.Remove(noteLabel);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}