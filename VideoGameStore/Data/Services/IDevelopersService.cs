using VideoGameStore.Data.Base;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public interface IDevelopersService: IEntityBaseRepository<Developer>
    {
        // All now inherited by IEntityBaseRepository

    }
}
