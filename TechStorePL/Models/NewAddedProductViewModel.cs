using TechStoreEL.ViewModels;

namespace TechStorePL.Models
{
    public class NewAddedProductViewModel
    {
        public long CategoryId { get; set; }
        public ProductDTO Product { get; set; }
        public ICollection<IFormFile>? ProductPicsFiles { get; set; }
        public string? ProductPicsURLS { get; set; } 
        public List<string>? ProductProps { get; set; } 
        public List<ProductPictureDTO>? ProductPictures { get; set; }
        //public List<CategoryProductPropertyDTO>? ProductProps { get; set; }
    }
}
