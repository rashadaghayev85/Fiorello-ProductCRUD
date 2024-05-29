using System.ComponentModel.DataAnnotations;

namespace FiorelloMVC.ViewModels.Blog
{
    public class BlogEditVM
    {
        public string? Image { get; set; }
        public IFormFile ?NewImage { get; set; }




        public string ? Title { get; set; }

        
        public string ? Description { get; set; }
    }
}
