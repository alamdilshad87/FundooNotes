using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBaseLayer.Repositories.Implementations
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooNotesDbContext _context;

        public LabelRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Label label)
        {
            await _context.Labels.AddAsync(label);
        }

        public async Task<List<Label>> GetAllAsync(int userId)
        {
            return await _context.Labels
                .Where(l => l.UserId == userId && !l.IsDeleted)
                .ToListAsync();
        }

        public async Task<Label?> GetByIdAsync(int labelId, int userId)
        {
            return await _context.Labels
                .FirstOrDefaultAsync(l =>
                    l.LabelId == labelId &&
                    l.UserId == userId &&
                    !l.IsDeleted);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}