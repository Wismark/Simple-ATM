using System.Linq;
using CM.Entites;

namespace CM.Services.interfaces
{
    public interface ICardRepository
    {
        IQueryable<Card> Cards { get; }
        bool BlockCard(string cardNumber);
        bool RegisterOperation(string cardNumber, OperationType type);
        bool Withdraw(string cardNumber, int sum);
        byte[] Hash(string value);
        int GetAttemptsNumber(string cardNumber);
        bool UpdateAttemptsNumber(string cardNumber, int num);
    }
}