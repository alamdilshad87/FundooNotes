using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Collaborators;
using ModelLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class CollaboratorService : ICollaboratorService
    {
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly IUserRepository _userRepository;
        private readonly INoteRepository _noteRepository;

        public CollaboratorService(
            ICollaboratorRepository collaboratorRepository,
            IUserRepository userRepository,
            INoteRepository noteRepository)
        {
            _collaboratorRepository = collaboratorRepository;
            _userRepository = userRepository;
            _noteRepository = noteRepository;
        }
        public async Task AddCollaboratorAsync(AddCollaboratorDto dto, int ownerId)
        {
            var note = await _noteRepository.GetByIdAsync(dto.NoteId, ownerId);
            if (note == null)
                throw new UnauthorizedException("Only note owner can add collaborators");

            var user = await _userRepository.GetByEmailAsync(dto.Email)
                ?? throw new NotFoundException("User not found");

            bool exists = await _collaboratorRepository.ExistsAsync(dto.NoteId, user.UserId);
            if (exists)
                throw new ValidationException("User already added as collaborator");

            var collaborator = new Collaborator
            {
                NoteId = dto.NoteId,
                UserId = user.UserId,
                Permission = dto.Permission
            };

            await _collaboratorRepository.AddAsync(collaborator);
            await _collaboratorRepository.SaveAsync();
        }
        public async Task<List<Collaborator>> GetCollaboratorsByNoteAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null)
                throw new NotFoundException("Note not found");

            return await _collaboratorRepository.GetByNoteIdAsync(noteId);
        }
        public async Task RemoveCollaboratorAsync(int collaboratorId, int ownerId)
        {
            var collaborator = await _collaboratorRepository.GetByIdAsync(collaboratorId)
                ?? throw new NotFoundException("Collaborator not found");

            var note = await _noteRepository.GetByIdAsync(collaborator.NoteId, ownerId);
            if (note == null)
                throw new UnauthorizedException("Only note owner can remove collaborators");

            await _collaboratorRepository.RemoveAsync(collaborator);
            await _collaboratorRepository.SaveAsync();
        }
        public async Task UpdatePermissionAsync(int collaboratorId, string permission, int ownerId)
        {
            var collaborator = await _collaboratorRepository.GetByIdAsync(collaboratorId)
                ?? throw new NotFoundException("Collaborator not found");

            var note = await _noteRepository.GetByIdAsync(collaborator.NoteId, ownerId);
            if (note == null)
                throw new UnauthorizedException("Only note owner can update permission");

            collaborator.Permission = permission;
            await _collaboratorRepository.SaveAsync();
        }
    }
}
