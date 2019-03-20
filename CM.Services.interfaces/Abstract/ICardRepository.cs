using Domain.Entities;
using System.Collections.Generic;

namespace CM.Services.interfaces.Abstract
{
    public interface ICardRepository
    {
        IEnumerable<Card> Cards { get; }
    }
}