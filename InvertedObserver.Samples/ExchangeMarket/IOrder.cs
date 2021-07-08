using System;
using System.Collections.Generic;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public interface IOrder
    {
        bool IsOpened { get; }
        DateTime OpenedAt { get; }
        decimal OpenPrice { get; }
        IEnumerable<(DateTime Timestamp, decimal Price)> PriceHistory { get; }
    }
}
