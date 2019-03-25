﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CM.Entites.Entities;

namespace CM.Services.interfaces.Abstract
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
