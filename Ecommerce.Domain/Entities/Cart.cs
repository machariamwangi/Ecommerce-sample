using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.Entities
{
    public class Cart
    {
        public Cart()
        {
            Count = 1;
        }

        [Key]
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        [ForeignKey("AppUserId")]
        public ApplicationUser AppUser { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

         
    }
}
