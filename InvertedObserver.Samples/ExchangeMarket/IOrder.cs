using System;
using System.Collections.Generic;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public interface IOrder
    {
        OrderStatus Status { get; }
        DateTime OpenTime { get; }
        decimal OpenPrice { get; }
        IEnumerable<(DateTime Timestamp, decimal Price)> PriceHistory { get; }
    }
}
