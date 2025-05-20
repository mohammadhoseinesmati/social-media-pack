
using social_media.Data.Models;
using static System.Net.Mime.MediaTypeNames;

namespace social_media.Data.Services
{
    public class FilesService : IFilesService
    {
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string rootfolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                var allowedContentTypes = new[] { "image/jpeg", "image/png" };
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string rootdirectorypath = Path.Combine(rootfolder, "images");

                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLower();
                string rootfile = Path.Combine(rootdirectorypath, filename);

                using (var stream = new FileStream(rootfile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return "/images/" + filename;
            }
            return "";
        }
    }
}
