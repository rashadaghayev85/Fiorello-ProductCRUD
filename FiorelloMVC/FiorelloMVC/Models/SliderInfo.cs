using System.ComponentModel.DataAnnotations;

namespace FiorelloMVC.Models
{
    public class SliderInfo:BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
