using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DataBaseLayer.Tests.Repositories
{
    public class NoteRepositoryTests
    {
        private FundooNotesDbContext _context;
        private NoteRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FundooNotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new FundooNotesDbContext(options);
            _repo = new NoteRepository(_context);
        }

        [Test]
        public async Task GetAllByUserAsync_Returns_Notes()
        {
            _context.Notes.Add(new Note { Title = "Test", UserId = 1 });
            await _context.SaveChangesAsync();

            var result = await _repo.GetAllByUserAsync(1);

            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}