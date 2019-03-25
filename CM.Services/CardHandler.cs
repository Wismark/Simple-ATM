﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CM.Entites.Entities;
using CM.Services.interfaces.Abstract;

namespace CM.Services
{
    public class CardHandler: ICardHandler
    {
        private readonly ICardRepository _repository;
        private string _cardNumber;

        public CardHandler(ICardRepository repo)
        {
            _repository = repo;
        }

        public bool CheckCard(string cardNumber)
        {
            var card = _repository.Cards.SingleOrDefault(c => c.CardNumber == cardNumber);
            if (card != null && !card.IsBlocked)
            {
                _cardNumber = cardNumber;
                return true;
            }
            return false;
        }

        public bool CheckPinCode(string pinCode, string cardNumber)
        {
            return _repository.Cards.Single(c => c.CardNumber == cardNumber).PinCode.SequenceEqual(_repository.Hash(pinCode));
        }

        public bool RegisterOperation(string cardNumber, OperationType type)
        {
            return _repository.RegisterOperation(cardNumber, type); 
        }

        public bool Withdraw(string cardNumber, int sum)
        {
            return _repository.Withdraw(cardNumber, sum);
        }

        public decimal GetBalance(string cardNumber)
        {
           return _repository.Cards.Single(c => c.CardNumber == cardNumber).Balance;
        }

        public bool BlockCard(string cardNumber)
        {
           return _repository.BlockCard(cardNumber);
        }
    }
}
