using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Notes;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteService.CreateNoteAsync(dto, userId);

            return Ok(new
            {
                message = "Note created successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
            var notes = await _noteService.GetAllNotesAsync(userId);

            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var note = await _noteService.GetNoteByIdAsync(id, userId);

            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDto dto)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteService.UpdateNoteAsync(id, dto, userId);

            return Ok(new
            {
                message = "Note updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteService.DeleteNoteAsync(id, userId);

            return Ok(new
            {
                message = "Note deleted successfully"
            });
        }

        [HttpPatch("{id}/pin")]
        public async Task<IActionResult> TogglePin(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteService.TogglePinAsync(id, userId);

            return Ok(new
            {
                message = "Pin status updated successfully"
            });
        }

        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ToggleArchive(int id)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteService.ToggleArchiveAsync(id, userId);

            return Ok(new
            {
                message = "Archive status updated successfully"
            });
        }
    }
}