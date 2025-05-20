namespace social_media.Data.Services
{
    public interface IFilesService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
