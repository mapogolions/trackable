using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class SellOrder : IOrder, IObserver<CurrencyPair>
    {
        private readonly decimal _supportLevel;
        private readonly decimal _takeProfit;
        private readonly List<(DateTime, decimal)> _priceHistory = new();
        private readonly IDisposable _registration;

        public SellOrder(decimal supportLevel, decimal takeProfit, CurrencyPair subject)
        {
            if (subject.CurrentPrice <= takeProfit) throw new ArgumentOutOfRangeException(nameof(takeProfit));
            _supportLevel = supportLevel;
            _takeProfit = takeProfit;
            Subject = subject;
            _registration = ChangeToken.OnChange(Subject.GetReloadToken, OnChangePrice);
            OnChangePrice();
        }

        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public DateTime OpenTime { get; private set; }
        public decimal OpenPrice { get; private set; }
        public IReadOnlyList<(DateTime Timestamp, decimal Price)> PriceHistory => _priceHistory;
        public CurrencyPair Subject { get; }

        private void OnChangePrice()
        {
            if (Status is OrderStatus.Closed) return;
            var utcNow = DateTime.UtcNow;
            var currentPrice = Subject.CurrentPrice;
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
