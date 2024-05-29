using FiorelloMVC.ViewModels.Categories;

namespace FiorelloMVC.ViewModels.Categories
{
    public class CategoryProductVM
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CreatedDate { get; set; }
        public int ProductCount { get; set; }
    }
}
