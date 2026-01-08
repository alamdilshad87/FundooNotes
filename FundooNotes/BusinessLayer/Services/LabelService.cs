using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task CreateLabelAsync(string name, int userId)
        {
            var label = new Label
            {
                Name = name,
                UserId = userId
            };

            await _labelRepository.AddAsync(label);
            await _labelRepository.SaveAsync();
        }

        public async Task<List<Label>> GetLabelsAsync(int userId)
        {
            return await _labelRepository.GetAllAsync(userId);
        }

        public async Task UpdateLabelAsync(int labelId, string name, int userId)
        {
            var label = await _labelRepository.GetByIdAsync(labelId, userId)
                ?? throw new NotFoundException("Label not found");

            label.Name = name;
            label.UpdatedAt = DateTime.UtcNow;

            await _labelRepository.SaveAsync();
        }

        public async Task DeleteLabelAsync(int labelId, int userId)
        {
            var label = await _labelRepository.GetByIdAsync(labelId, userId)
                ?? throw new NotFoundException("Label not found");

            label.IsDeleted = true;
            label.UpdatedAt = DateTime.UtcNow;

            await _labelRepository.SaveAsync();
        }
    }
}