using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VideoGameStore.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context){ }

    }
}
