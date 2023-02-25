using System.ComponentModel;

namespace VideoGameStore.Data.Enums
{
    public enum GameAgeRating
    {
        [Description("Everyone")]
        E =1,
        [Description("Everyone 10+")]
        E10,
        [Description("Teen")]
        T,
        [Description("Mature")]
        M,
        [Description("Adults Only")]
        AO,
        [Description("Rating Pending")]
        RP
    }
}
