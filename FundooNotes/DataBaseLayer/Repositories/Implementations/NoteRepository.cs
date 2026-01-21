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

        public async Task<Note?> GetByIdAsync(int noteId, int userId)
        {
            return await _context.Notes
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .FirstOrDefaultAsync(n => n.NoteId == noteId && n.UserId == userId);
        }

        public async Task<List<Note>> GetAllByUserAsync(int userId)
        {
            return await _context.Notes
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.UpdatedAt)
                .ToListAsync();
        }

        public async Task<List<Note>> GetByIdsAsync(List<int> noteIds, int userId)
        {
            return await _context.Notes
                .Where(n => noteIds.Contains(n.NoteId) && n.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Note>> SearchAsync(int userId, string query)
        {
            return await _context.Notes
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .Where(n => n.UserId == userId &&
                    (n.Title.Contains(query) || n.Content.Contains(query)))
                .ToListAsync();
        }

        public async Task AddAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
        }

        // ✅ ADD THIS METHOD
        public async Task DeleteAsync(Note note)
        {
            _context.Notes.Remove(note);
            await Task.CompletedTask; // For async signature
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
