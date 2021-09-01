using System.Threading;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class CurrencyPair : IObservable
    {
        private CancellationTokenSource _cts = new();
        private decimal _currentPrice;

        public CurrencyPair(string name, decimal currentPrice)
        {
            Name = name;
            _currentPrice = currentPrice;
        }

        public string Name { get; }

        public decimal CurrentPrice
        {
            get => _currentPrice;
            set
            {
                _currentPrice = value;
                RefreshToken();
            }
        }

        public IChangeToken GetReloadToken() => new CancellationChangeToken(_cts.Token);

        private void RefreshToken()
        {
            var cts = _cts;
            _cts = new();
            cts.Cancel();
        }
    }
}
