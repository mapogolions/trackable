using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class BuyOrder : IOrder, IObserver<CurrencyPair>
    {
        private readonly decimal _resistanceLevel;
        private readonly List<(DateTime, decimal)> _priceHistory = new();

        public BuyOrder(decimal resistanceLevel, CurrencyPair subject)
        {
            _resistanceLevel = resistanceLevel;
            Subject = subject;
            ChangeToken.OnChange(subject.GetReloadToken, OnChangePrice);
            OnChangePrice();
        }

        public bool IsOpened { get; private set; }
        public DateTime OpenedAt { get; private set; }
        public decimal OpenPrice { get; private set; }
        public IEnumerable<(DateTime Timestamp, decimal Price)> PriceHistory => _priceHistory;

        public CurrencyPair Subject { get; }

        private void OnChangePrice()
        {
            _priceHistory.Add((DateTime.Now, Subject.CurrentPrice));
            if (IsOpened) return;
            IsOpened = Subject.CurrentPrice > _resistanceLevel;
            if (IsOpened)
            {
                OpenedAt = DateTime.Now;
                OpenPrice = Subject.CurrentPrice;
            }
        }
    }
}
