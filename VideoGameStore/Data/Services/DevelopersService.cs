using VideoGameStore.Data.Base;
using VideoGameStore.Models;

namespace VideoGameStore.Data.Services
{
    public class DevelopersService : EntityBaseRepository<Developer>, IDevelopersService
    {
        public DevelopersService(AppDbContext context) : base(context){ }

    }
}
