using System;
using Trackable.Samples.Forex;
using Xunit;

namespace Trackable.Tests.Forex
{
    public class ForexWatcherTests
    {
        [Fact]
        public void ShouldStopWatchingMarket()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/JPY", 157.13m);
            var forexWatcher = new ForexWatcher(usdJpy, gbpJpy);

            forexWatcher.Dispose();
            gbpJpy.CurrentPrice = 154m;

            Assert.Single(forexWatcher.Journal);
        }

        [Fact]
        public void ShouldCaptureMultipleChanges()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/JPY", 157.13m);
            using var forexWatcher = new ForexWatcher(usdJpy, gbpJpy);

            usdJpy.CurrentPrice = 121m;
            gbpJpy.CurrentPrice = 154m;

            Assert.Equal(3, forexWatcher.Journal.Count);
        }

        [Fact]
        public void ShouldJournalPriceChange()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/JPY", 157.13m);
            using var forexWatcher = new ForexWatcher(usdJpy, gbpJpy);

            usdJpy.CurrentPrice = 121m;

            Assert.Equal(2, forexWatcher.Journal.Count);
        }


        [Fact]
        public void ShouldDump()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/JPY", 157.13m);
            using var forexWatcher = new ForexWatcher(usdJpy, gbpJpy);

            var expected = "USD/JPY 119.21"
                         + Environment.NewLine
                         + "GBP/JPY 157.13";

            Assert.EndsWith(expected, forexWatcher.ToString());
        }

        [Fact]
        public void ShouldCaptureTheStartPrice()
        {
            var usdJpy = new CurrencyPair("USD/JPY", 119.21m);
            var gbpJpy = new CurrencyPair("GBP/JPY", 157.13m);
            using var forexWatcher = new ForexWatcher(usdJpy, gbpJpy);

            Assert.Single(forexWatcher.Journal);
        }
    }
}
