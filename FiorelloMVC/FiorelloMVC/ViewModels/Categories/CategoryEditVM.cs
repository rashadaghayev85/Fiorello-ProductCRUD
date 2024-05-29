using System.ComponentModel.DataAnnotations;

namespace FiorelloMVC.ViewModels.Categories
{
    public class CategoryEditVM
    {
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
