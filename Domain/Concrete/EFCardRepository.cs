using Domain.Entities;
using System.Collections.Generic;
using CM.Services.interfaces.Abstract;

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