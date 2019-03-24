using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Services.interfaces.Abstract
{
    public  interface ICardHandler
    {
        bool CheckPinCode(string pin, string card);
        bool CheckCard(string card);
        void BlockCard(string cardNumber);
    }
}
