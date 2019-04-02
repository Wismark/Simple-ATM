using CM.Entites;

namespace CM.Services.interfaces
{
    public  interface ICard
    {
        bool CheckPinCode(string pin, string card);
        bool CheckCard(string card);
        bool BlockCard(string cardNumber);
        bool RegisterOperation(string cardNumber, OperationType type);
        bool Withdraw(string cardNumber, int sum);
        decimal GetBalance(string cardNumber);
        int GetAttemptsNumber(string cardNumber);
        bool UpdateAttemptsNumber(string cardNumber, int num);
    }
}
