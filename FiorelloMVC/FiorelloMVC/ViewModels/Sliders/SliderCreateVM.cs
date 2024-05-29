using System.ComponentModel.DataAnnotations;

namespace FiorelloMVC.ViewModels.Sliders
{
    public class SliderCreateVM
    {
        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
