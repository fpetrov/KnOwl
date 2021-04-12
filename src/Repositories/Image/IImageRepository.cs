using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Repositories.Image
{
    public interface IImageRepository
    {
        Task<List<IFormFile>> Add(List<IFormFile> entities);
        bool Remove(string path);
    }
}