using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class MarketWatcher : GroupObservable<CurrencyPair>, IObserver<CurrencyPair>, IDisposable
    {
        private readonly CurrencyPair[] _currencies;
        private readonly List<(DateTime, string[])> _journal = new();
        private readonly IDisposable _registration;

        public MarketWatcher(params CurrencyPair[] currencies) : base(currencies)
        {
            _currencies = currencies;
            _registration = ChangeToken.OnChange(GetReloadToken, OnChange);
            OnChange();
        }

        public IReadOnlyList<(DateTime, string[])> Journal => _journal;

        public void Dispose() => _registration.Dispose();

        private void OnChange()
        {
            var utcNow = DateTime.UtcNow;
            var info = _currencies.Select(x => $"{x.Name} {x.CurrentPrice}").ToArray();
            _journal.Add((utcNow, info));
        }

        public override string ToString()
        {
            return _journal
                .Aggregate(string.Empty,
                    (acc, x) =>
                        $"{acc}{x.Item1}{Environment.NewLine}{string.Join(Environment.NewLine, x.Item2)}")
                .Trim();
        }
    }
}
