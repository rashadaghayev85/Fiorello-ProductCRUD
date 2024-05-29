using NuGet.Protocol.Core.Types;

namespace FiorelloMVC.ViewModels.Product
{
    public class ProductImageVM
    {
        public int  Id { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; }
    }
}
