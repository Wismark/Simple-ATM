using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
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

        public void BlockCard(string cardNumber)
        {
            _context.Cards.Single(c => c.CardNumber == cardNumber).IsBlocked = true;
            _context.SaveChanges();
        }
    }
}