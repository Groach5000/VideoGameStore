using System.ComponentModel.DataAnnotations;

namespace VideoGameStore.Data.Enums
{
    public enum PriceRange
    {
        [Display(Name = "Free to play")]
        Free = 0,
        [Display(Name = "Under $5.00")]
        u5 = 5,
        [Display(Name = "Under $10.00")]
        u10 = 10,
        [Display(Name = "Under $15.00")]
        u15 = 15,
        [Display(Name = "Under $20.00")]
        u20 = 20,
        [Display(Name = "Under $25.00")]
        u25 = 25,
        [Display(Name = "Under $30.00")]
        u30 = 30,
        [Display(Name = "Under $35.00")]
        u35 = 35,
        [Display(Name = "Under $40.00")]
        u40 = 40,
        [Display(Name = "Under $45.00")]
        u45 = 45,
        [Display(Name = "Under $50.00")]
        u50 = 50,
        [Display(Name = "Under $55.00")]
        u55 = 55,
        [Display(Name = "Under $60.00")]
        u60 = 60,
        //[Display(Name = "No Max")]
        //NoMax = 999
    }
}