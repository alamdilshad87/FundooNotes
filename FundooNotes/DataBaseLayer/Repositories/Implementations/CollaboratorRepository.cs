using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBaseLayer.Repositories.Implementations
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly FundooNotesDbContext _context;

        public CollaboratorRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Collaborator collaborator)
        {
            await _context.Collaborators.AddAsync(collaborator);
        }

        public async Task<bool> ExistsAsync(int noteId, int userId)
        {
            return await _context.Collaborators
                .AnyAsync(c => c.NoteId == noteId && c.UserId == userId);
        }

        public async Task<List<Collaborator>> GetByNoteIdAsync(int noteId)
        {
            return await _context.Collaborators
                .Where(c => c.NoteId == noteId)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<Collaborator?> GetByIdAsync(int collaboratorId)
        {
            return await _context.Collaborators
                .FirstOrDefaultAsync(c => c.CollaboratorId == collaboratorId);
        }

        public async Task RemoveAsync(Collaborator collaborator)
        {
            _context.Collaborators.Remove(collaborator);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}