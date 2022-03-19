using System;
using System.Linq;
using System.Threading.Tasks;
using InvertedObserver.Samples.ExchangeMarket;
using Xunit;

namespace InvertedObserver.Tests.ExchangeMarket
{
    public class OrdersTests
    {
        [Fact]
        public void ShouldCloseOrderAndStopJournalPriceHistory()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 100.0m);
            var buyOrder = new BuyOrder(resistanceLevel: 102m, takeProfit: 110m, currencyPair: usdJpy);

            usdJpy.CurrentPrice = 103m;
            usdJpy.CurrentPrice = 113;

            Assert.Equal(OrderStatus.Closed, buyOrder.Status);
            Assert.Equal(3, buyOrder.PriceHistory.Count);
        }

        [Fact]
        public void ShouldThrowExceptionIfTakeProfitIsLessThanCurrencyPairPrice()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 100.0m);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SellOrder(supportLevel: 102m, takeProfit: 101m, currencyPair: usdJpy));
        }

        [Fact]
        public void ShouldThrowExceptionIfTakeProfitIsGreaterThanCurrencyPairPrice()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 100.0m);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new BuyOrder(resistanceLevel: 98m, takeProfit: 99m, currencyPair: usdJpy));
        }

        [Fact]
        public void OrdersRegistrationShouldNotAffectPriceHistoryOfEachOther()
        {

            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 100.0m);
            var buyOrder = new BuyOrder(resistanceLevel: 109.50m, takeProfit: 110m, currencyPair: usdJpy);
            var sellOrder = new SellOrder(supportLevel: 98.0m, takeProfit: 95m, currencyPair: usdJpy);

            Assert.Single(buyOrder.PriceHistory);
            Assert.Single(sellOrder.PriceHistory);

            usdJpy.CurrentPrice = 101m;

            Assert.Equal(2, buyOrder.PriceHistory.Count());
            Assert.Equal(2, sellOrder.PriceHistory.Count());
        }

        [Fact]
        public void ShouldJournalPriceHistory()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var buyOrder = new BuyOrder(resistanceLevel: 109.50m, takeProfit: 114m, currencyPair: usdJpy);

            usdJpy.CurrentPrice = 108.7m;
            usdJpy.CurrentPrice = 108.8m;
            usdJpy.CurrentPrice = 108.9m;

            Assert.Equal(4, buyOrder.PriceHistory.Count());
            Assert.Equal(108.41m, buyOrder.PriceHistory.FirstOrDefault().Price);
        }

         [Fact]
        public async Task OrderShouldBeOpenOnlyOnce()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108m);
            var sellOrder = new SellOrder(supportLevel: 105m, takeProfit: 101m,currencyPair: usdJpy);

            usdJpy.CurrentPrice = 106m;
            usdJpy.CurrentPrice = 104m;
            var checkpoint = DateTime.Now;
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            usdJpy.CurrentPrice = 103m;

            Assert.True(sellOrder.OpenTime < checkpoint);
            Assert.Equal(104m, sellOrder.OpenPrice);
        }

        [Fact]
        public void BuyOrderShouldBeNotifiedAtTheTimeOfAttachment()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 113.5m);
            var buyOrder = new BuyOrder(resistanceLevel: 109m, takeProfit: 114m, currencyPair: usdJpy);

            Assert.Equal(OrderStatus.Open, buyOrder.Status);
            Assert.Equal(usdJpy.CurrentPrice, buyOrder.OpenPrice);
        }

        [Fact]
        public void SellOrderShouldBeNotifiedAtTheTimeOfAttachment()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var sellOrder = new SellOrder(supportLevel: 109m, takeProfit: 100m, currencyPair: usdJpy);

            Assert.Equal(OrderStatus.Open, sellOrder.Status);
            Assert.Equal(usdJpy.CurrentPrice, sellOrder.OpenPrice);
        }

        [Fact]
        public void ShouldNotifyWhenCurrentPriceLessThanSupportLevel()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var sellOrder = new SellOrder(supportLevel: 107.00m, takeProfit: 105m, currencyPair: usdJpy);

            Assert.Equal(OrderStatus.Pending, sellOrder.Status);
            usdJpy.CurrentPrice = 106.90m;
            Assert.Equal(OrderStatus.Open, sellOrder.Status);
        }

        [Fact]
        public void ShouldNotifyWhenCurrentPriceGreaterThanResistanceLevel()
        {
            var usdJpy = new CurrencyPair(name: "USD/JPY", currentPrice: 108.41m);
            var buyOrder = new BuyOrder(resistanceLevel: 109.50m, takeProfit: 113m, currencyPair: usdJpy);

            Assert.Equal(OrderStatus.Pending, buyOrder.Status);
            usdJpy.CurrentPrice = 110.04m;
            Assert.Equal(OrderStatus.Open, buyOrder.Status);
        }
    }
}
