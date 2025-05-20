using social_media.Data.DTOS;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public interface IFriendsServices
    {
        Task SendREquestAsync(int senderid, int reciverid);
        Task<FriendRequest> UpdateREquestAsync(int reciverid , string status);
        Task RemoveFriendshipAsync(int firendship);
        Task<List<UserWithFriendsDTO>> GetSuggestedFriendsAsync(int userid);
        Task<List<FriendRequest>> GetRequestFriendsAsync(int userid);
        Task<List<FriendRequest>> GetReciveRequestFriendsAsync(int userid);
        Task<List<Friendship>> GetFriendshipListAsync(int userid);
    }
}
