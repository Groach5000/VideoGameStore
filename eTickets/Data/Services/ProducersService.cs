using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VideoGameStore.Data.Services
{
    public class ProducersService : EntityBaseRepository<Producer>, IProducersService
    {
        public ProducersService(AppDbContext context) : base(context){ }

    }
}
