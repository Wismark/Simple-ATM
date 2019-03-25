using System.Collections.Generic;
using System.Linq;
using CM.Entites.Entities;

namespace CM.Services.interfaces.Abstract
{
    public interface ICardRepository
    {
        IQueryable<Card> Cards { get; }
        bool BlockCard(string cardNumber);
        bool RegisterOperation(string cardNumber, OperationType type);
        bool Withdraw(string cardNumber, int sum);
    }
}