using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Services.Image
{
    public interface IImageService
    {
        Task<List<Entities.Storage.Image>> GetAll();
        Task<Entities.Storage.Image> Get(int id);
        Task<List<Entities.Storage.Image>> Add(List<IFormFile> images);
        Task<Entities.Storage.Image> Remove(int id);
    }
}
