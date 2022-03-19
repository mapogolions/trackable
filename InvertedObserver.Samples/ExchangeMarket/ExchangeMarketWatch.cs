using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class ExchangeMarketWatch : IObserver<CurrencyPair>, IDisposable
    {
        private readonly CurrencyPair[] _currencies;
        private readonly List<(string, decimal, DateTime)[]> _journal = new();
        private readonly IDisposable _registration;

        public ExchangeMarketWatch(params CurrencyPair[] currencies)
        {
            _currencies = currencies;
            var groupObservable = new GroupObservable<CurrencyPair>(currencies);
            _registration = ChangeToken.OnChange(groupObservable.GetReloadToken, OnChange);
            OnChange();
        }

        public void Dispose() => _registration.Dispose();

        private void OnChange()
        {
            _journal.Add(_currencies.Select(x => (x.Name, x.CurrentPrice, DateTime.UtcNow)).ToArray());
        }
    }
}
