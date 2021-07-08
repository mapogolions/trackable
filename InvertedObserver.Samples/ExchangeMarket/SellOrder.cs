using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class SellOrder : IOrder, IObserver<CurrencyPair>
    {
        private readonly decimal _supportLevel;
        private readonly List<(DateTime, decimal)> _priceHistory = new();

        public SellOrder(decimal supportLevel, CurrencyPair subject)
        {
            _supportLevel = supportLevel;
            Subject = subject;
            ChangeToken.OnChange(Subject.GetReloadToken, OnChangePrice);
            Subject.RefreshToken();
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
            IsOpened = Subject.CurrentPrice < _supportLevel;
            if (IsOpened)
            {
                OpenedAt = DateTime.Now;
                OpenPrice = Subject.CurrentPrice;
            }
        }
    }
}
