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
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.CreateNoteAsync(dto, userId);
            return Ok(new { message = "Note created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            List<NoteResponseDto> notes = await _noteService.GetAllNotesAsync(userId);

            var activeNotes = notes.Where(n => !n.IsDeleted && !n.IsArchived).ToList();

            return Ok(activeNotes);
        }

        [HttpGet("trash")]
        public async Task<IActionResult> GetTrashedNotes()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            List<NoteResponseDto> notes = await _noteService.GetTrashedNotesAsync(userId);
            return Ok(notes);
        }

        [HttpGet("archive")]
        public async Task<IActionResult> GetArchivedNotes()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            List<NoteResponseDto> notes = await _noteService.GetArchivedNotesAsync(userId);
            return Ok(notes);
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreNote(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.RestoreNoteAsync(id, userId);
            return Ok(new { message = "Note restored successfully" });
        }

        [HttpDelete("{id}/permanent")]
        public async Task<IActionResult> PermanentDeleteNote(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.PermanentDeleteNoteAsync(id, userId);
            return Ok(new { message = "Note permanently deleted" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            NoteResponseDto note = await _noteService.GetNoteByIdAsync(id, userId);
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.UpdateNoteAsync(id, dto, userId);
            return Ok(new { message = "Note updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.DeleteNoteAsync(id, userId);
            return Ok(new { message = "Note moved to trash" });
        }

        [HttpPatch("{id}/pin")]
        public async Task<IActionResult> TogglePin(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.TogglePinAsync(id, userId);
            return Ok(new { message = "Pin status updated successfully" });
        }

        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ToggleArchive(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.ToggleArchiveAsync(id, userId);
            return Ok(new { message = "Archive status updated successfully" });
        }

        [HttpPatch("{id}/color")]
        public async Task<IActionResult> UpdateColor(int id, [FromBody] UpdateNoteColorDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.UpdateNoteColorAsync(id, dto, userId);
            return Ok(new { message = "Note color updated successfully" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchNotes([FromQuery] string query)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            List<NoteResponseDto> notes = await _noteService.SearchNotesAsync(userId, query);
            return Ok(notes);
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.BulkDeleteAsync(dto.NoteIds, userId);
            return Ok(new { message = "Notes moved to trash" });
        }

        [HttpPost("from-template/{templateId}")]
        public async Task<IActionResult> CreateFromTemplate(int templateId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _noteService.CreateFromTemplateAsync(templateId, userId);
            return Ok(new { message = "Note created from template" });
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetNoteHistory(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var history = await _noteService.GetNoteHistoryAsync(id, userId);
            return Ok(history);
        }
    }
}
