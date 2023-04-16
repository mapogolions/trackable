using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Trackable.Samples.Forex
{
    public class SellOrder : IOrder, ITracker<CurrencyPair>
    {
        private readonly decimal _supportLevel;
        private readonly decimal _takeProfit;
        private readonly CurrencyPair _currencyPair;
        private readonly List<(DateTime, decimal)> _priceHistory = new();
        private readonly IDisposable _registration;

        public SellOrder(decimal supportLevel, decimal takeProfit, CurrencyPair currencyPair)
        {
            if (currencyPair.CurrentPrice <= takeProfit) throw new ArgumentOutOfRangeException(nameof(takeProfit));
            _supportLevel = supportLevel;
            _takeProfit = takeProfit;
            _currencyPair = currencyPair;
            _registration = ChangeToken.OnChange(_currencyPair.GetToken, OnChangePrice);
            OnChangePrice();
        }

        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public DateTime OpenTime { get; private set; }
        public decimal OpenPrice { get; private set; }
        public IReadOnlyList<(DateTime Timestamp, decimal Price)> PriceHistory => _priceHistory;

        CurrencyPair ITracker<CurrencyPair>.Subject => _currencyPair;

        private void OnChangePrice()
        {
            if (Status is OrderStatus.Closed) return;
            var utcNow = DateTime.UtcNow;
            var currentPrice = _currencyPair.CurrentPrice;
            _priceHistory.Add((utcNow, currentPrice));
            if (Status is OrderStatus.Open)
            {
                if (currentPrice <= _takeProfit)
                {
                    _registration.Dispose();
                    Status = OrderStatus.Closed;
                }
                return;
            }
            if (currentPrice < _supportLevel)
            {
                OpenTime = utcNow;
                OpenPrice = currentPrice;
                Status = OrderStatus.Open;
            }
        }
    }
}
