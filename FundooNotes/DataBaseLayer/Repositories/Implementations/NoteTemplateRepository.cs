using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBaseLayer.Repositories.Implementations
{
    public class NoteTemplateRepository : INoteTemplateRepository
    {
        private readonly FundooNotesDbContext _context;

        public NoteTemplateRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task<NoteTemplate?> GetByIdAsync(int templateId)
        {
            return await _context.NoteTemplates
                .FirstOrDefaultAsync(t => t.TemplateId == templateId && t.IsActive);
        }
    }
}