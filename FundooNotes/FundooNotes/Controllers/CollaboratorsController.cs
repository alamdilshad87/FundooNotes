using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Collaborators;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/collaborators")]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollaboratorService _service;

        public CollaboratorsController(ICollaboratorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCollaboratorDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _service.AddCollaboratorAsync(dto, userId);
            return Ok(new { message = "Collaborator added" });
        }

        [HttpGet("note/{noteId}")]
        public async Task<IActionResult> GetByNote(int noteId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.GetCollaboratorsByNoteAsync(noteId, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _service.RemoveCollaboratorAsync(id, userId);
            return Ok(new { message = "Collaborator removed" });
        }

        [HttpPatch("{id}/permission")]
        public async Task<IActionResult> UpdatePermission(int id, UpdatePermissionDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _service.UpdatePermissionAsync(id, dto.Permission, userId);
            return Ok(new { message = "Permission updated" });
        }
    }
}