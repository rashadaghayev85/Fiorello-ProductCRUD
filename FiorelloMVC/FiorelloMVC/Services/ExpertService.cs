using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Services
{
    public class ExpertService:IExpertService
    {
        private readonly AppDBContext _context;
        public ExpertService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expert>> GetAllAsync()
        {
            return await _context.Experts.Where(m => !m.SoftDeleted).ToListAsync();
        }
    }
}
