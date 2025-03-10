using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.DTO.Blog;
using SkinCare_Service.Service;
using System.Security.Claims;

namespace SkinCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogs();
            return Ok(blogs);
        }

        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetBlogById(string blogId)
        {
            var blog = await _blogService.GetBlogById(blogId);
            if (blog == null) return NotFound("Blog not found");
            return Ok(blog);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newBlog = await _blogService.CreateBlog(userId, dto);
            return CreatedAtAction(nameof(GetBlogById), new { blogId = newBlog.BlogId }, newBlog);
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("update/{blogId}")]
        public async Task<IActionResult> UpdateBlog(string blogId, [FromBody] UpdateBlogDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _blogService.UpdateBlog(blogId, dto, userId);
            if (!result) return NotFound("Blog not found or you are not authorized to update this blog");
            return NoContent();
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("delete/{blogId}")]
        public async Task<IActionResult> DeleteBlog(string blogId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _blogService.DeleteBlog(blogId, userId);
            if (!result) return NotFound("Blog not found or you are not authorized to delete this blog");
            return NoContent();
        }
    }
}
