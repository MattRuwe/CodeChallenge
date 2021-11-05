using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmahaMTG.Challenge.Challenges;

namespace OmahaMTG.Sample.StockTraderImplementation
{
    public class StockTrader : IStockTraderChallenge
    {

        public string AuthorNotes
        {
            //Author notes are not allowed in this challenge
            get { return string.Empty; }
        }

        //Provides a place to store the prices that we've receive
        private List<decimal> _history;

        public void Reset()
        {
            //Do any setup needed here - your entry is not reconstructed between runs
            _history = new List<decimal>();
        }

        public DayAction PerformStockAction(DayStock stock, Portfolio portfolio)
        {
            //This function is called once for every day of the stock
            //Store any information you need between calls as a class level variable (the constructor for this class is only called once)
            DayAction returnValue = new DayAction();
            if (_history.Count > 0)
            {
                if (_history.Last() > stock.Close)
                {
                    //The price is going down, so it's time to sell some stock
                    returnValue.Action = TradeAction.Sell;
                    returnValue.Quantity = Convert.ToInt32(portfolio.SharesOwns * 0.5);
                }
                else if (_history.Last() < stock.Close)
                {
                    //The price is going up, so it's time to buy some stock
                    returnValue.Action = TradeAction.Buy;
                    returnValue.Quantity = Convert.ToInt32((portfolio.Cash * 0.1M) / stock.Close);
                }
            }

            _history.Add(stock.Close);

            return returnValue;
        }

    }
}
