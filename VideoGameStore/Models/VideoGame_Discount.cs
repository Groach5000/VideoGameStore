using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Models
{
    public class VideoGame_Discount
    {
        public VideoGame_Discount()
        {
            this.VideoGames = new HashSet<VideoGame>();
        }

        [Key]
        public int Id { get; set; }

        public virtual ICollection<VideoGame> VideoGames { get; set; }

        public double DiscountValue { get; set; }
        public string DiscountUnit { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpiry { get; set; }
        public bool IsActive { get; set; }
    }
}
