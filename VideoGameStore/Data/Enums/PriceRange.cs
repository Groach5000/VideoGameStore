using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Data.Enums
{
    public enum PriceRange
    {
        [Display(Name = "Free to play")]
        Free = 0,
        [Display(Name = "$5.00")]
        d5 = 5,
        [Display(Name = "$10.00")]
        d10 = 10,
        [Display(Name = "$15.00")]
        d15 = 15,
        [Display(Name = "$20.00")]
        d20 = 20,
        [Display(Name = "$25.00")]
        d25 = 25,
        [Display(Name = "$30.00")]
        d30 = 30,
        [Display(Name = "$35.00")]
        d35 = 35,
        [Display(Name = "$40.00")]
        d40 = 40,
        [Display(Name = "$45.00")]
        d45 = 45,
        [Display(Name = "$50.00")]
        d50 = 50,
        [Display(Name = "$55.00")]
        d55 = 55,
        [Display(Name = "$60.00")]
        d60 = 60,
        [Display(Name = "No Max")]
        NoMax = 9999
    }
}