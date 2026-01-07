using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DataBaseLayer.Repositories.Implementations
{
    public class NoteRepository : INoteRepository
    {
        private readonly FundooNotesDbContext _context;

        public NoteRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
        }

        public async Task<List<Note>> GetAllByUserAsync(int userId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int noteId, int userId)
        {
            return await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsDeleted);
        }

        public Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}