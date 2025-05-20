namespace social_media.Data.Services
{
    public interface IHashtagService
    {
        Task ProccessHashtagForNewPostAsync(string content);
        Task ProccessHashtagForRemovePostAsync(string content);
    }
}
