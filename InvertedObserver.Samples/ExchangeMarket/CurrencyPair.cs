using System.Threading;

namespace InvertedObserver.Samples.ExchangeMarket
{
    public class CurrencyPair : Observable
    {
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
    }
}
