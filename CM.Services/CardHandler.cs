using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CM.Services.interfaces.Abstract;

namespace CM.Services
{
    public class CardHandler
    {
        private ICardRepository _repository;

        public CardHandler(ICardRepository repo)
        {
            _repository = repo;
        }

        public bool CheckCard(string cardNumber)
        {
            return _repository.Cards.SingleOrDefault(c => c.CardNumber == cardNumber) != null;
        }
    }
}
