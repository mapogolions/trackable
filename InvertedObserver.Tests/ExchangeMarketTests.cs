using System;
using System.Linq;
using System.Threading.Tasks;
using InvertedObserver.Samples.ExchangeMarket;
using Xunit;

namespace InvertedObserver.Tests
{
    public class ExchangeMarketTests
    {
        [Fact]
        public void ShouldJournalPriceHistory()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var buyOrder = new BuyOrder(resistanceLevel: 109.50m, subject: usdJpy);

            usdJpy.CurrentPrice = 108.7m;
            usdJpy.CurrentPrice = 108.8m;
            usdJpy.CurrentPrice = 108.9m;

            Assert.False(buyOrder.IsOpened);
            Assert.Equal(4, buyOrder.PriceHistory.Count());
            Assert.Equal(108.41m, buyOrder.PriceHistory.FirstOrDefault().Price);
        }

         [Fact]
        public async Task OrderShouldBeOpenedOnlyOnce()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108m);
            var sellOrder = new SellOrder(supportLevel: 105m, subject: usdJpy);

            usdJpy.CurrentPrice = 106m;
            usdJpy.CurrentPrice = 104m;
            var checkpoint = DateTime.Now;
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            usdJpy.CurrentPrice = 103m;

            Assert.True(sellOrder.OpenedAt < checkpoint);
            Assert.Equal(104m, sellOrder.OpenPrice);
        }

        [Fact]
        public void BuyOrderShouldBeNotifiedAtTheTimeOfAttachment()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 113.5m);
            var buyOrder = new BuyOrder(resistanceLevel: 109m, subject: usdJpy);

            Assert.True(buyOrder.IsOpened);
            Assert.Equal(usdJpy.CurrentPrice, buyOrder.OpenPrice);
        }

        [Fact]
        public void SellOrderShouldBeNotifiedAtTheTimeOfAttachment()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var sellOrder = new SellOrder(supportLevel: 109m, subject: usdJpy);

            Assert.True(sellOrder.IsOpened);
            Assert.Equal(usdJpy.CurrentPrice, sellOrder.OpenPrice);
        }

        [Fact]
        public void ShouldNotifyWhenCurrentRateLessThanSupportLevel()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var sellOrder = new SellOrder(supportLevel: 107.00m, subject: usdJpy);

            Assert.False(sellOrder.IsOpened);
            usdJpy.CurrentPrice = 106.90m;
            Assert.True(sellOrder.IsOpened);
        }

        [Fact]
        public void ShouldNotifyWhenCurrentRateGreaterThanResistanceLevel()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var buyOrder = new BuyOrder(resistanceLevel: 109.50m, subject: usdJpy);

            Assert.False(buyOrder.IsOpened);
            usdJpy.CurrentPrice = 110.04m;
            Assert.True(buyOrder.IsOpened);
        }
    }
}
