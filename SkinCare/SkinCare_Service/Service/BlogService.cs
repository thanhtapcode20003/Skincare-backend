using Microsoft.EntityFrameworkCore;
using SkinCare_Data;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Blog;

namespace SkinCare_Service.Service
{
    public class BlogService
    {
        private readonly SkinCare_DBContext _context;

        public BlogService(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogDto>> GetAllBlogs()
        {
            return await _context.Blogs
                .Select(b => new BlogDto
                {
                    BlogId = b.BlogId,
                    UserId = b.UserId,
                    CategoryId = b.CategoryId,
                    Title = b.Title,
                    Content = b.Content,
                    PublishDate = b.PublishDate
                })
                .ToListAsync();
        }

        public async Task<BlogDto> GetBlogById(string blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null) return null;

            return new BlogDto
            {
                BlogId = blog.BlogId,
                UserId = blog.UserId,
                CategoryId = blog.CategoryId,
                Title = blog.Title,
                Content = blog.Content,
                PublishDate = blog.PublishDate
            };
        }

        public async Task<Blog> CreateBlog(string userId, CreateBlogDto dto)
        {
            var newBlog = new Blog
            {
                BlogId = Guid.NewGuid().ToString(),
                UserId = userId,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Content = dto.Content,
                PublishDate = DateTime.UtcNow
            };

            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();
            return newBlog;
        }

        public async Task<bool> UpdateBlog(string blogId, UpdateBlogDto dto, string userId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null || blog.UserId != userId) return false;

            blog.Title = dto.Title;
            blog.Content = dto.Content;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBlog(string blogId, string userId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null || blog.UserId != userId) return false;

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
