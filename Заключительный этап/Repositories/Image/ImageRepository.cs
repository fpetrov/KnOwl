using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KnOwl.Repositories.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<IFormFile>> Add(List<IFormFile> entities)
        {
            foreach (var formFile in entities)
            {
                if (formFile.Length > 0)
                {
                    var storagePath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["ClientRootPath"], _configuration["StorageRootPath"]);

                    if (!Directory.Exists(storagePath))
                        Directory.CreateDirectory(storagePath);

                    var filePath = Path.Combine(storagePath, formFile.FileName);

                    if (File.Exists(filePath))
                        return null;

                    using (var stream = File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return entities;
        }

        public bool Remove(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);

                return true;
            }

            return false;
        }
    }
}
