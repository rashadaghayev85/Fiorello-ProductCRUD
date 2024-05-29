using FiorelloMVC.Models;
using FiorelloMVC.ViewModels.Blog;
namespace FiorelloMVC.ViewModels
{
    public class HomeVM
    {
        
        public IEnumerable<BlogVM> Blogs { get; set; }
        public IEnumerable<Expert>Experts { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Models.Product> Products { get; set; }
    }
}
