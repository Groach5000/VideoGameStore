using VideoGameStore.Data.Base;
using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VideoGameStore.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context){ }

    }
}
