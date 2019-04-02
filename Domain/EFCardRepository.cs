using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CM.Entites;
using CM.Services.interfaces;

namespace Domain
{
    public class EfCardRepository : ICardRepository
    {
        readonly EfDbContext _context = new EfDbContext();
        public IQueryable<Card> Cards => _context.Cards;

        public bool BlockCard(string cardNumber)
        {
            try
            {
                _context.Cards.Single(c => c.CardNumber == cardNumber).AttemptsCount = 0;
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool RegisterOperation(string cardNumber, OperationType type)
        {
            try
            {
                var operation = new Operation()
                {
                    OperationDate = DateTime.Now,
                    OperationType = type,
                    CardId = _context.Cards.Single(c => c.CardNumber == cardNumber).CardId
                };

                _context.Operation.Add(operation);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Withdraw(string cardNumber, int sum)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var balance = _context.Cards.Single(c=>c.CardNumber==cardNumber).Balance;
                if (sum > balance && sum > 0) return false;
                {
                    balance = balance - sum;
                    _context.Cards.Single(c => c.CardNumber == cardNumber).Balance = balance;
                   
                    var operation = new Operation()
                    {
                        OperationDate = DateTime.Now,
                        OperationType = OperationType.Withdraw,
                        CardId = _context.Cards.Single(c => c.CardNumber == cardNumber).CardId,
                        WithdrawSum = sum
                    };

                    _context.Operation.Add(operation);
                    _context.SaveChanges();

                    transaction.Commit();                       
                    return true;
                }
            }
        }

        public int GetAttemptsNumber(string cardNumber)
        {
            return _context.Cards.Single(c => c.CardNumber == cardNumber).AttemptsCount;
        }

        public bool UpdateAttemptsNumber(string cardNumber, int num)
        {
            if (num >= 0)
            {
                _context.Cards.Single(c => c.CardNumber == cardNumber).AttemptsCount = num;
                return true;
            }
            return false;
        }

        public byte[] Hash(string value)
        {
            byte[] salt = Encoding.ASCII.GetBytes("YYLmfY6IehjZMQbv5PehSMfV11CdQxLUF1bgIAdeQX");
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        private byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            // Alternatively use CopyTo.
            //var saltedValue = new byte[value.Length + salt.Length];
            //value.CopyTo(saltedValue, 0);
            //salt.CopyTo(saltedValue, value.Length);
            return new SHA256Managed().ComputeHash(saltedValue);
        }
    }
}