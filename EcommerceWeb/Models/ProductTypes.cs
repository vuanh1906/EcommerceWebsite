using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }

        [Required]
        public string ProductType { get; set; }

    }
}
