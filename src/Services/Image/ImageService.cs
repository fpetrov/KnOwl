using KnOwl.Database.Contexts;
using KnOwl.Repositories.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnOwl.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly ApplicationContext _context;
        private readonly IImageRepository _imageRepository;

        public ImageService(ApplicationContext context, IImageRepository imageRepository)
        {
            _context = context;
            _imageRepository = imageRepository;
        }

        public async Task<List<Entities.Storage.Image>> Add(List<IFormFile> images)
        {
            var imageCollection = images.Select(collection => new Entities.Storage.Image { Id = default, Name = collection.FileName });

            var response = await _imageRepository.Add(images);

            if (response == null)
                return null;

            foreach (var item in imageCollection)
                _context.Set<Entities.Storage.Image>().Add(item);

            await _context.SaveChangesAsync();

            return imageCollection.ToList();
        }

        public async Task<Entities.Storage.Image> Get(int id)
        {
            return await _context.Set<Entities.Storage.Image>().FindAsync(id);
        }

        public async Task<List<Entities.Storage.Image>> GetAll()
        {
            return await _context.Set<Entities.Storage.Image>().ToListAsync();
        }

        public async Task<Entities.Storage.Image> Remove(int id)
        {
            var image = await _context.Set<Entities.Storage.Image>().FindAsync(id);

            if (image == null)
                return image;

            _imageRepository.Remove(image.Name);

            _context.Set<Entities.Storage.Image>().Remove(image);
            await _context.SaveChangesAsync();

            return image;
        }
    }
}
