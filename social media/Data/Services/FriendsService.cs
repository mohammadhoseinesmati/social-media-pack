
using Microsoft.EntityFrameworkCore;
using social_media.Data.DTOS;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public class FriendsService : IFriendsServices
    {
        private readonly AppDBContext _dbcontext;
        
        public FriendsService(AppDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<FriendRequest> UpdateREquestAsync(int requestId, string status)
        {
            var request = await _dbcontext.FriendRequests.FirstOrDefaultAsync(p => p.Id == requestId);
            if (request != null)
            {
                request.Status = status;
                request.Updated = DateTime.Now;
                _dbcontext.Update(request);
                await _dbcontext.SaveChangesAsync();
            }
            if (status == "Accepted")
            {
                var friendship = new Friendship
                {
                    SenderId = request.SenderId,
                    ReciverId = request.ReciverId,
                    Created = DateTime.Now
                };
                await _dbcontext.Friendships.AddAsync(friendship);
                await _dbcontext.SaveChangesAsync();
            }
            return request;
        }

        public async Task SendREquestAsync(int senderid, int reciverid)
        {
            var request = new FriendRequest
            {
                ReciverId = reciverid,
                Status = "Pending",
                SenderId = senderid,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };
            _dbcontext.FriendRequests.Add(request);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<List<UserWithFriendsDTO>> GetSuggestedFriendsAsync(int userid)
        {
            var existedfriendIds = await _dbcontext.Friendships
                .Where(p => p.SenderId == userid || p.ReciverId == userid)
                .Select(n => n.SenderId == userid ? n.ReciverId : n.SenderId)
                .ToListAsync();

            var pendingrequestid = await _dbcontext.FriendRequests
                .Where(n => (n.SenderId == userid || n.ReciverId == userid) && n.Status == "Pending")
                .Select(n => n.SenderId == userid ? n.ReciverId : n.SenderId)
                .ToListAsync();

            var suggestedfriends = await _dbcontext.Users
                .Where(p => p.Id != userid &&
                !existedfriendIds.Contains(p.Id)&&
                !pendingrequestid.Contains(p.Id))
                .Select(user => new UserWithFriendsDTO
                {
                    User = user,
                    FriendsCount = _dbcontext.Friendships
                    .Count(f => f.SenderId == user.Id || f.ReciverId == user.Id)
                })
                .Take(5)
                .ToListAsync();

            return suggestedfriends;
        }
        public async Task RemoveFriendshipAsync(int firendship)
        {
            var friendship = await _dbcontext.Friendships.FirstOrDefaultAsync(p => p.Id == firendship);

            if (friendship != null)
            {
                _dbcontext.Friendships.Remove(friendship);
                await _dbcontext.SaveChangesAsync();
            }
        }
        public async Task<List<FriendRequest>> GetRequestFriendsAsync(int userid)
        {
            var friendsrequestlist = await _dbcontext.FriendRequests
                .Include(x => x.Reciver)
                .Include(x => x.Sender)
                .Where(x => x.SenderId == userid && x.Status == "Pending")
                .ToListAsync();

            return friendsrequestlist;
        }
        public async Task<List<FriendRequest>> GetReciveRequestFriendsAsync(int userid)
        {
            var friendshiprecive = await _dbcontext.FriendRequests
                .Include (x => x.Reciver)
                .Include(x => x.Sender)
                .Where(x => x.ReciverId ==  userid && x.Status == "Pending")
                .ToListAsync ();

            return friendshiprecive;
        }
        public async Task<List<Friendship>> GetFriendshipListAsync(int userid)
        {
            var friendship = await _dbcontext.Friendships
                .Include(x => x.Reciver)
                .Include(x => x.Sender)
                .Where(x => x.ReciverId == userid || x.SenderId == userid)
                .ToListAsync();

            return friendship;
        }
    }
}
