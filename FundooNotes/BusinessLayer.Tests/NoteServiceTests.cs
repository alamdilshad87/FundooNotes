using BusinessLayer.Services;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Notes;
using Moq;
using NUnit.Framework;

namespace BusinessLayer.Tests.Services
{
    public class NoteServiceTests
    {
        private Mock<INoteRepository> _noteRepo;
        private Mock<INoteTemplateRepository> _templateRepo;
        private Mock<INoteHistoryRepository> _historyRepo;
        private NoteService _service;

        [SetUp]
        public void Setup()
        {
            _noteRepo = new Mock<INoteRepository>();
            _templateRepo = new Mock<INoteTemplateRepository>();
            _historyRepo = new Mock<INoteHistoryRepository>();

            _service = new NoteService(
                _noteRepo.Object,
                _templateRepo.Object,
                _historyRepo.Object
            );
        }

        [Test]
        public async Task CreateNoteAsync_Should_Add_Note()
        {
            var dto = new CreateNoteDto
            {
                Title = "Test",
                Content = "Content",
                Color = "white"
            };

            await _service.CreateNoteAsync(dto, 1);

            _noteRepo.Verify(r => r.AddAsync(It.IsAny<Note>()), Times.Once);
            _noteRepo.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}