using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notes")]
    public class NoteLabelsController : ControllerBase
    {
        private readonly INoteLabelService _noteLabelService;

        public NoteLabelsController(INoteLabelService noteLabelService)
        {
            _noteLabelService = noteLabelService;
        }

        [HttpPost("{noteId}/labels/{labelId}")]
        public async Task<IActionResult> AddLabel(int noteId, int labelId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteLabelService.AddLabelToNoteAsync(noteId, labelId, userId);
            return Ok(new { message = "Label added to note" });
        }

        [HttpDelete("{noteId}/labels/{labelId}")]
        public async Task<IActionResult> RemoveLabel(int noteId, int labelId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _noteLabelService.RemoveLabelFromNoteAsync(noteId, labelId, userId);
            return Ok(new { message = "Label removed from note" });
        }

        [HttpGet("{noteId}/labels")]
        public async Task<IActionResult> GetLabels(int noteId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var labels = await _noteLabelService.GetLabelsByNoteAsync(noteId, userId);
            return Ok(labels);
        }
    }
}
