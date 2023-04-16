using System;
using System.Collections.Generic;

namespace Trackable.Samples.Forex
{
    public interface IOrder
    {
        OrderStatus Status { get; }
        DateTime OpenTime { get; }
        decimal OpenPrice { get; }
        IReadOnlyList<(DateTime Timestamp, decimal Price)> PriceHistory { get; }
    }
}
