using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CM.Entites;

namespace Domain
{
    public class EfDbContext : DbContext
    {
        public EfDbContext() : base("DbConnection")
        {
           // Database.SetInitializer(new EfDbContextInitializer());
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Operation> Operation { get; set; }
    }


    public class EfDbContextInitializer : DropCreateDatabaseAlways<EfDbContext>
    {
        protected override void Seed(EfDbContext db)
        {
            var card1 = new Card()
            {
                Balance = 200.12m,
                CardNumber = "5555555555555555",
                IsBlocked = false,
                PinCode = Hash("1111")
            };
            var card2 = new Card()
            {
                Balance = 0.12m,
                CardNumber = "1555555555555555",
                IsBlocked = false,
                PinCode = Hash("2222")
            }; 
            var card3 = new Card()
            {
                Balance = 999999m,
                CardNumber = "2222222222222222",
                IsBlocked = false,
                PinCode = Hash("1212")
            }; 

            db.Cards.AddRange(new [] { card1, card2, card3 });
            db.SaveChanges();
        }

        public byte[] Hash(string value)
        {
            byte[] salt = Encoding.ASCII.GetBytes("YYLmfY6IehjZMQbv5PehSMfV11CdQxLUF1bgIAdeQX");
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        private byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            return new SHA256Managed().ComputeHash(saltedValue);
        }
    }
}