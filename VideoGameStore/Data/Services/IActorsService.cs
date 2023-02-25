using VideoGameStore.Data.Base;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public interface IActorsService: IEntityBaseRepository<Actor>
    {
        // All now inherited by IEntityBaseRepository
    }
}
