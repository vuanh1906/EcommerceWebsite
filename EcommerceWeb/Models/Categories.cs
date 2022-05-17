using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Models
{
    public class Categories
    {
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
