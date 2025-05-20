
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using social_media.Data.Helpers;
using social_media.Data.Models;

namespace social_media.Data.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly AppDBContext _dbContext;
        public HashtagService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ProccessHashtagForNewPostAsync(string content)
        {
            var hashtags = HashtagHelper.GetHashtags(content);

            foreach (var hashtag in hashtags)
            {
                var hashtagDB = await _dbContext.Hashtags.FirstOrDefaultAsync(p => p.Name == hashtag);
                if (hashtagDB != null)
                {
                    hashtagDB.Count += 1;
                    hashtagDB.Updated = DateTime.UtcNow;

                    _dbContext.Hashtags.Update(hashtagDB);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    var newHashtag = new Hashtag()
                    {
                        Name = hashtag,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow,
                        Count = 1
                    };

                    await _dbContext.Hashtags.AddAsync(newHashtag);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task ProccessHashtagForRemovePostAsync(string content)
        {
            var hashtags = HashtagHelper.GetHashtags(content);

            foreach (var hashtag in hashtags)
            {
                var hashtagDB = _dbContext.Hashtags.FirstOrDefault(x => x.Name == hashtag);
                if (hashtagDB != null)
                {
                    hashtagDB.Count -= 1;
                    hashtagDB.Updated = DateTime.UtcNow;
                    if (hashtagDB.Count == 0)
                    {
                        _dbContext.Hashtags.Remove(hashtagDB);
                    }
                    else
                    {
                        _dbContext.Hashtags.Update(hashtagDB);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
