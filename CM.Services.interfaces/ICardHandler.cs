using CM.Entites;

namespace CM.Services.interfaces
{
    public  interface ICardHandler
    {
        bool CheckPinCode(string pin, string card);
        bool CheckCard(string card);
        bool BlockCard(string cardNumber);
        bool RegisterOperation(string cardNumber, OperationType type);
        bool Withdraw(string cardNumber, int sum);
        decimal GetBalance(string cardNumber);
    }
}
