using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBaseLayer.Repositories.Implementations
{
    public class NoteHistoryRepository : INoteHistoryRepository
    {
        private readonly FundooNotesDbContext _context;

        public NoteHistoryRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NoteHistory history)
        {
            await _context.NoteHistories.AddAsync(history);
        }

        public async Task<List<NoteHistory>> GetByNoteIdAsync(int noteId, int userId)
        {
            return await _context.NoteHistories
                .Where(h => h.NoteId == noteId && h.UserId == userId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
