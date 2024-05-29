using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly AppDBContext _context;
        public SocialMediaService(AppDBContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<SocialMedia>>GetAllAsync()
        {
            return await _context.SocialMedias.ToListAsync();
        }
    }
}
