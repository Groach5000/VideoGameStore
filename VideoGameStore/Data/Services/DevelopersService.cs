using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VideoGameStore.Data.Services
{
    public class DevelopersService : EntityBaseRepository<Developer>, IDevelopersService
    {
        public DevelopersService(AppDbContext context) : base(context){ }

    }
}
