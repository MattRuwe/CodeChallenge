using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmahaMTG.Sample.StockTraderImplementation;
using OmahaMTG.Challenge.Challenges;
using System.Net;
using System.Diagnostics;

namespace StockTraderTest
{
    [TestClass()]
    public class StockTraderTest
    {
        [TestMethod()]
        public void StockTraderExecutionTest()
        {
            List<DayStock> dailyStockData = GetStockPrices("msft", new DateTime(1986, 3, 13), DateTime.Today);

            StockTrader stockTrader = new StockTrader();
            Portfolio portfolio = new Portfolio
            {
                Cash = 100000,
                SharesOwns = 0
            };

            stockTrader.Reset();

            foreach (DayStock dailyStock in dailyStockData)
            {
                DayAction action = stockTrader.PerformStockAction(dailyStock, portfolio);
                if (action != null && action.Quantity > 0)
                {
                    switch (action.Action)
                    {
                        case TradeAction.Buy:
                            decimal cost = dailyStock.Close * action.Quantity;
                            if (portfolio.Cash - cost >= 0)
                            {
                                portfolio.Cash -= cost;
                                portfolio.SharesOwns += action.Quantity;
                            }
                            break;
                        case TradeAction.Sell:
                            decimal income = dailyStock.Close * action.Quantity;
                            if (action.Quantity <= portfolio.SharesOwns)
                            {
                                portfolio.Cash += income;
                                portfolio.SharesOwns -= action.Quantity;
                            }
                            break;
                    }
                }
            }

            //Sell the remaining stock
            portfolio.Cash += portfolio.SharesOwns * dailyStockData.Last().Close;
            portfolio.SharesOwns = 0;

            Debug.WriteLine(string.Format("Finished with a portfolio value of {0:C}.", portfolio.Cash));
        }

        private List<DayStock> GetStockPrices(string tickerSymbol, DateTime startDate, DateTime endDate)
        {
            WebClient request = new WebClient();

            //Get date range for stock values here (replace MSFT with the ticker symbol you want):
            //http://finance.yahoo.com/q/hp?s=MSFT+Historical+Prices

            string requestUrl = string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&d={1}&e={2}&f={3}&g=d&a={4}&b={5}&c={6}&ignore=.csv", tickerSymbol, endDate.Month - 1, endDate.Day, endDate.Year, startDate.Month - 1, startDate.Day, startDate.Year);

            string response = Encoding.UTF8.GetString(request.DownloadData(requestUrl));
            response = response.Replace("\n\r", "\n").Replace("\r", "\n");
            string[] lines = response.Split('\n');

            List<DayStock> returnValue = new List<DayStock>();

            int count = 0;

            foreach (string line in lines.Skip(1).Reverse())
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    count += 1;
                    string[] items = line.Split(',');

                    DayStock dayStock = new DayStock();
                    dayStock.DayNumber = count;
                    dayStock.Open = decimal.Parse(items[1]);
                    dayStock.High = decimal.Parse(items[2]);
                    dayStock.Low = decimal.Parse(items[3]);
                    dayStock.Volume = int.Parse(items[5]);
                    dayStock.Close = decimal.Parse(items[6]);

                    returnValue.Add(dayStock);
                }
            }

            return returnValue;
        }

    }

}
