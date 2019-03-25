﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CM.Entites.Entities;
using CM.Services.interfaces.Abstract;

namespace Domain.Concrete
{
    public class EfCardRepository : ICardRepository
    {
        readonly EfDbContext _context = new EfDbContext();
        public IQueryable<Card> Cards
        {
            get { return _context.Cards; }
        }
        public bool BlockCard(string cardNumber)
        {
            try
            {
                _context.Cards.Single(c => c.CardNumber == cardNumber).IsBlocked = true;
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
                try
                {
                    var balance = _context.Cards.Single(c=>c.CardNumber==cardNumber).Balance;
                    if (sum <= balance) 
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
                    throw new Exception();
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

    }
}