using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace EcommerceWeb.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        public double Price { get; set; }

        [Display(Name = "Image Name")]
        public string? ImageName { get; set; }   

        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }
        
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes? ProductTypes { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Categories? Categories { get; set; }


        [NotMapped]
        [Display(Name ="Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
