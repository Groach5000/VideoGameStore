using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Data.Enums
{
    public enum GameGenre
    {
        [Display(Name = "Action")]
        Action = 1,
        [Display(Name = "Indie")]
        Indie,
        [Display(Name = "Adventure")]
        Adventure,
        [Display(Name = "Role Playing Game")]
        RPG,
        [Display(Name = "Strategy")]
        Strategy,
        [Display(Name = "Shooter")]
        Shooter,
        [Display(Name = "Casual")]
        Casual,
        [Display(Name = "Simulation")]
        Simulation,
        [Display(Name = "Puzzle")]
        Puzzle,
        [Display(Name = "Arcade")]
        Arcade,
        [Display(Name = "Platformer")]
        Platformer,
        [Display(Name = "Racing")]
        Racing,
        [Display(Name = "Massively Multiplayer")]
        Massively_Multiplayer,
        [Display(Name = "Sports")]
        Sports,
        [Display(Name = "Fighting")]
        Fighting,
        [Display(Name = "Family")]
        Family,
        [Display(Name = "Board Game")]
        Board_Games,
        [Display(Name = "Educational")]
        Educational,
        [Display(Name = "Card")]
        Card
    }
}
