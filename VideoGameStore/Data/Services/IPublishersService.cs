using VideoGameStore.Data.Base;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public interface IPublishersService: IEntityBaseRepository<Publisher>
    {
        // All now inherited by IEntityBaseRepository
    }
}
