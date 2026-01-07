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
    }
}