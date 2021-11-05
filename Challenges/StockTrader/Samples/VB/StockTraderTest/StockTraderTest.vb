Imports System.Net
Imports System.Text
Imports OmahaMTG.Challenge.Challenges
Imports OmahaMTG.Sample.StockTraderImplementation

<TestClass()>
Public Class StockTraderTest

    <TestMethod()>
    Public Sub StockTraderExecutionTest()
        Dim dailyStockData As List(Of DayStock) = GetStockPrices("msft", New DateTime(1986, 3, 13), DateTime.Today)

        Dim stockTrader As New StockTrader
        Dim portfolio As New Portfolio With
            {
                .Cash = 100000,
                .SharesOwns = 0
            }

        stockTrader.Reset()

        For Each dailyStock As DayStock In dailyStockData
            Dim action As DayAction = stockTrader.PerformStockAction(dailyStock, portfolio)
            If action IsNot Nothing AndAlso action.Quantity > 0 Then
                Select Case action.Action
                    Case TradeAction.Buy
                        Dim cost As Decimal = dailyStock.Close * action.Quantity
                        If portfolio.Cash - cost >= 0 Then
                            portfolio.Cash -= cost
                            portfolio.SharesOwns += action.Quantity
                        End If
                    Case TradeAction.Sell
                        Dim income As Decimal = dailyStock.Close * action.Quantity
                        If action.Quantity <= portfolio.SharesOwns Then
                            portfolio.Cash += income
                            portfolio.SharesOwns -= action.Quantity
                        End If
                End Select
            End If
        Next

        'Sell the remaining stock
        portfolio.Cash += portfolio.SharesOwns * dailyStockData.Last.Close
        portfolio.SharesOwns = 0

        Debug.WriteLine(String.Format("Finished with a portfolio value of {0:C}.", portfolio.Cash))
    End Sub

    Private Function GetStockPrices(tickerSymbol As String, startDate As DateTime, endDate As DateTime) As List(Of DayStock)
        Dim request As New WebClient

        'Get date range for stock values here (replace MSFT with the ticker symbol you want):
        'http://finance.yahoo.com/q/hp?s=MSFT+Historical+Prices

        Dim requestUrl As String = String.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&d={1}&e={2}&f={3}&g=d&a={4}&b={5}&c={6}&ignore=.csv",
                                                 tickerSymbol,
                                                 endDate.Month - 1,
                                                 endDate.Day,
                                                 endDate.Year,
                                                 startDate.Month - 1,
                                                 startDate.Day,
                                                 startDate.Year)

        Dim response As String = Encoding.UTF8.GetString(request.DownloadData(requestUrl))
        response = response.Replace(vbCrLf, vbCr).Replace(vbLf, vbCr)
        Dim lines As String() = Split(response, vbCr)

        Dim returnValue As New List(Of DayStock)

        Dim count As Integer = 0

        For Each line As String In lines.Skip(1).Reverse()
            If Not String.IsNullOrWhiteSpace(line) Then
                count += 1
                Dim items As String() = Split(line, ",")

                Dim dayStock As New DayStock
                With dayStock
                    .DayNumber = count
                    .Open = Decimal.Parse(items(1))
                    .High = Decimal.Parse(items(2))
                    .Low = Decimal.Parse(items(3))
                    .Volume = Integer.Parse(items(5))
                    .Close = Decimal.Parse(items(6))
                End With

                returnValue.Add(dayStock)
            End If
        Next

        Return returnValue
    End Function

End Class
