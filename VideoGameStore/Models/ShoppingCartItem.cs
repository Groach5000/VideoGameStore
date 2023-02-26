using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        public VideoGame VideoGame { get; set; }

        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
