using System;
using InvertedObserver.Samples.ExchangeMarket;
using Xunit;

namespace InvertedObserver.Tests.ExchangeMarket
{
    public class MarketWatchTests
    {
        [Fact]
        public void ShouldStopWatchingMarket()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/USD", 157.13m);
            var marketWatcher = new MarketWatcher(usdJpy, gbpJpy);

            marketWatcher.Dispose();
            gbpJpy.CurrentPrice = 154m;

            Assert.Single(marketWatcher.Journal);
        }

        [Fact]
        public void ShouldCaptureMultipleChanges()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/USD", 157.13m);
            using var marketWatcher = new MarketWatcher(usdJpy, gbpJpy);

            usdJpy.CurrentPrice = 121m;
            gbpJpy.CurrentPrice = 154m;

            Assert.Equal(3, marketWatcher.Journal.Count);
        }

        [Fact]
        public void ShouldJournalPriceChange()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/USD", 157.13m);
            using var marketWatcher = new MarketWatcher(usdJpy, gbpJpy);

            usdJpy.CurrentPrice = 121m;

            Assert.Equal(2, marketWatcher.Journal.Count);
        }


        [Fact]
        public void ShouldDump()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/USD", 157.13m);
            using var marketWatcher = new MarketWatcher(usdJpy, gbpJpy);

            var expected = "USD/JPY 119.21"
                         + Environment.NewLine
                         + "GBP/USD 157.13";

            Assert.EndsWith(expected, marketWatcher.ToString());
        }

        [Fact]
        public void ShouldCaptureTheStartPrice()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/USD", 157.13m);
            using var marketWatcher = new MarketWatcher(usdJpy, gbpJpy);

            Assert.Single(marketWatcher.Journal);
        }
    }
}
