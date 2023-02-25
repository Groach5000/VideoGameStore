using VideoGameStore.Data.Base;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public interface IProducersService: IEntityBaseRepository<Producer>
    {
        // All now inherited by IEntityBaseRepository

    }
}
