using social_media.Data.DTOS;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessage>> Messages(int userGoal);
        Task<List<ChatContactDto>> GetContectsAsync(int userGoal);
    }
}
