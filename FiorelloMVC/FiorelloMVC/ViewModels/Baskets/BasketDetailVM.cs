namespace FiorelloMVC.ViewModels.Baskets
{
    public class BasketDetailVM
    {
        public List<BasketProductVM> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalCount { get; set; }
    }
}
