using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels.Blog;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FiorelloMVC.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDBContext _context;
        public BlogService(AppDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Blog blog)
        {
            await _context.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Blog blog)
        {
            _context.Remove(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string title)
        {
            return await _context.Blogs.AnyAsync(m => m.Title.Trim() == title.Trim());
        }

        public async Task<IEnumerable<BlogVM>> GetAllAsync(int? take=null)
        {
            IEnumerable<Blog> blogs;
            //IEnumerable<Blog> blogs = await _context.Blogs.ToListAsync();
            ////List<BlogVM> datas = new();
            ////foreach (var blog in blogs)
            ////{
            ////    datas.Add(new BlogVM { Title = blog.Title });   
            ////}

            //var datas = blogs.Select(m => new BlogVM { Title = m.Title, Description = m.Description, Image = m.Image, CreatedDate = m.CreatedDate.ToString("MM.dd.yyyy") });
            if (take is null)
            {
                blogs= await _context.Blogs.ToListAsync();
            }
            else
            {
                 blogs= await _context.Blogs.Take((int)take).ToListAsync();
            }
            return blogs.Select(m => new BlogVM {Id=m.Id, Title = m.Title, Description = m.Description, Image = m.Image, CreatedDate = m.CreatedDate.ToString("MM.dd.yyyy") }); 
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.Where(m=>m.Id==id).FirstOrDefaultAsync();
        }
    }
}
