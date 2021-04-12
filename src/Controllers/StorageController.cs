using KnOwl.Models.Storage;
using KnOwl.Services;
using KnOwl.Services.Article;
using KnOwl.Services.Image;
using KnOwl.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnOwl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;

        public StorageController(IImageService imageService, IArticleService articleService, IUserService userService)
        {
            _imageService = imageService;
            _articleService = articleService;
            _userService = userService;
        }

        // Articles.
        [HttpPost("test")]
        public IActionResult Test()
        {
            var response = HttpContext.User.Claims.Where(c => c.Type == "name").FirstOrDefault();

            if (response == null)
                return BadRequest(new { message = "Can't add this article!" });

            return Ok(new { message = response.Value });
        }

        [HttpPost("article")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleRequest request)
        {
            request.Author = await _userService.Get(HttpContext.GetUserName());

            var response = await _articleService.Add(request);

            if (response == null)
                return BadRequest(new { message = "Can't add this article!" });

            return Ok(new { message = "Article was saved successfuly!" });
        }

        // Images.
        [HttpPost("image")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateImage(List<IFormFile> files)
        {
            var response = await _imageService.Add(files);

            if (response == null)
                return BadRequest(new { message = "File with this name already exists!" });

            return Ok(new { message = "New image was saved successfuly!" });
        }

        [HttpDelete("image/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            await _imageService.Remove(id);

            return Ok(new { message = "Image was deleted succeseffuly!" });
        }
    }
}
