using System.Collections.Generic;

namespace CM.Entites
{
    public class Card
    {
        public int CardId { get; set; }

        public string CardNumber { get; set; }
        
        public int AttemptsCount { get; set; }

        public byte[] PinCode { get; set; }

        public  decimal Balance { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
