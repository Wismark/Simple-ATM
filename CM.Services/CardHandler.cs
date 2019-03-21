using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CM.Services.interfaces.Abstract;
using Domain.Entities;

namespace CM.Services
{
    public class CardHandler
    {
        private readonly ICardRepository _repository;
        private short _attemptsCount = 4;

        public CardHandler(ICardRepository repo)
        {
            _repository = repo;
        }

        public bool CheckCard(string cardNumber)
        {
            var card = _repository.Cards.SingleOrDefault(c => c.CardNumber == cardNumber);
            return card != null && !card.IsBlocked;
        }

        public bool CheckPinCode(string pinCode, string cardNumber)
        {
            if (_repository.Cards.Single(c => c.CardNumber == cardNumber).PinCode == pinCode)
            {
                return true;
            }
            _attemptsCount--;
            if (_attemptsCount == 0) _repository.BlockCard(cardNumber);
            return false;
        }
    }
}
