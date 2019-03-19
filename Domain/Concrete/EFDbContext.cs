using Domain.Entities;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class EfDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
    }
}