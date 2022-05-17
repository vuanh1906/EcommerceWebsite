using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWeb.Models
{
    public class Comment 
    {
        public int Id { get; set; }

        public string author { get; set; }

        [StringLength(2048,MinimumLength = 4, ErrorMessage ="The minium length is 4 characters and maximum is 2048")]
        public string content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Product")]
        public int ProductsId { get; set; }

        [Display(Name = "ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ProductsId")]
        public virtual Products Products { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        

    }
}
