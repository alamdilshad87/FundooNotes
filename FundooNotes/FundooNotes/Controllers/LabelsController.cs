using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/labels")]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLabels()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _labelService.GetLabelsAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] string name)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _labelService.CreateLabelAsync(name, userId);
            return Ok(new { message = "Label created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(int id, [FromBody] string name)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _labelService.UpdateLabelAsync(id, name, userId);
            return Ok(new { message = "Label updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _labelService.DeleteLabelAsync(id, userId);
            return Ok(new { message = "Label deleted successfully" });
        }
    }
}