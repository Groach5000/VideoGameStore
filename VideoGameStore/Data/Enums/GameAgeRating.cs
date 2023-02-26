using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VideoGameStore.Data.Enums
{
    public enum GameAgeRating
    {
        [Display(Name = "Everyone")]
        E = 1,
        [Display(Name = "Everyone 10+")]
        E10,
        [Display(Name = "Teen")]
        T,
        [Display(Name = "Mature")]
        M,
        [Display(Name = "Adults Only")]
        AO,
        [Display(Name = "Rating Pending")]
        RP
    }
}