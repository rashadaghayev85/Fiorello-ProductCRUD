using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDBContext _context;
        public SliderService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<SliderInfo> GetSliderInfoAsync()
        {
            return await _context.SliderInfos.Where(m => !m.SoftDeleted).FirstOrDefaultAsync();
        }
    }
}
