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
                .AsNoTracking()
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int noteId, int userId)
        {
            return await _context.Notes
                .AsNoTracking()
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
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

        public async Task<List<Note>> SearchAsync(int userId, string query)
        {
            return await _context.Notes
                .AsNoTracking()
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .Where(n =>
                    n.UserId == userId &&
                    !n.IsDeleted &&
                    (n.Title.Contains(query) || n.Content.Contains(query))
                )
                .OrderByDescending(n => n.UpdatedAt)
                .ToListAsync();
        }

        public async Task<List<Note>> GetByIdsAsync(List<int> noteIds, int userId)
        {
            return await _context.Notes
                .AsNoTracking()
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .Where(n =>
                    noteIds.Contains(n.NoteId) &&
                    n.UserId == userId &&
                    !n.IsDeleted
                )
                .ToListAsync();
        }
    }
}