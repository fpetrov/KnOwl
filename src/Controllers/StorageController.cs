using KnOwl.Models.Storage;
using KnOwl.Services;
using KnOwl.Services.Article;
using KnOwl.Services.Image;
using KnOwl.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [HttpPost("articles")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleRequest request)
        {
            request.Article.Author = await _userService.Get(HttpContext.GetUserName());

            var response = await _articleService.Add(request);

            if (response == null)
                return BadRequest(new { message = "Can't add this article!" });

            return Ok(new { message = "Article was saved successfuly!" });
        }

        [HttpGet("articles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticles()
        {
            var response = await _articleService.GetAll();

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("articles/take/{count}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticlesByCount(int? count)
        {
            var response = await _articleService.Take(count);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("articles/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var response = await _articleService.Get(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("articles/private")]
        public async Task<IActionResult> GetPrivateArticles()
        {
            var author = await _userService.Get(HttpContext.GetUserName());
            var response = author.Articles;

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        // Images.
        [HttpPost("images")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateImage(List<IFormFile> files)
        {
            var response = await _imageService.Add(files);

            if (response == null)
                return BadRequest(new { message = "File with this name already exists!" });

            return Ok(new { message = "New image was saved successfuly!" });
        }

        [HttpDelete("images/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            await _imageService.Remove(id);

            return Ok(new { message = "Image was deleted succeseffuly!" });
        }
    }
}
