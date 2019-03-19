using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Concrete
{
    public class EfCardRepository : ICardRepository
    {
        readonly EfDbContext _context = new EfDbContext();
        public IEnumerable<Card> Cards
        {
            get { return _context.Cards; }
        }
    }
}