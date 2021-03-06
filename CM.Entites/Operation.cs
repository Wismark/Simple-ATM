﻿using System;

namespace CM.Entites
{
    public enum OperationType { Balance, Withdraw };
    public class Operation
    {
        public int OperationId { get; set; }
        public int CardId { get; set; }
        public  DateTime OperationDate { get; set; }

        public OperationType OperationType { get; set; }

        public int? WithdrawSum { get; set; }
    }
}
