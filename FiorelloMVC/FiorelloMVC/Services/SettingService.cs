using FiorelloMVC.Data;
using FiorelloMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDBContext _context;
        public SettingService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            return await _context.Settings.ToDictionaryAsync(m=>m.Key,m=>m.Value);
        }
    }
}
