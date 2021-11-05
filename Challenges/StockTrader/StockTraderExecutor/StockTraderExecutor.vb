Imports OmahaMTG.Challenge.ExecutionCommon
Imports System.Reflection
Imports System.IO
Imports System.Text

Public Class StockTraderExecutor
    Inherits ChallengeExecutorBase(Of IStockTraderChallenge)

    Protected Overrides Sub RunChallengeOverride()
        Dim dayStocks As List(Of DayStock)

        Dim stockChoices As IEnumerable(Of Integer)

        Dim stockTicker As String = String.Empty

        If IsTest Then
            stockChoices = {11, 12, 13}
        Else
            stockChoices = Enumerable.Range(1, 10).OrderBy(Function(x) Guid.NewGuid)
        End If


        For Each stockChoice As Integer In stockChoices
            Select Case stockChoice
                Case 1
                    stockTicker = "arna"
                Case 2
                    stockTicker = "fslr"
                Case 3
                    stockTicker = "brkb"
                Case 4
                    stockTicker = "nflx"
                Case 5
                    stockTicker = "oww"
                Case 6
                    stockTicker = "pcln"
                Case 7
                    stockTicker = "rsog"
                Case 8
                    stockTicker = "trlg"
                Case 9
                    stockTicker = "tzoo"
                Case 10
                    stockTicker = "vdsi"
                Case 11
                    stockTicker = "msft"
                Case 12
                    stockTicker = "goog"
                Case 13
                    stockTicker = "aapl"
            End Select

            If Not String.IsNullOrWhiteSpace(stockTicker) Then
                Console.WriteLine("Executing for stock {0}", stockTicker)
                RunSingleChallenge(stockTicker, GetDailyStockValues(stockTicker))
            End If
        Next
    End Sub

    Private Sub RunSingleChallenge(stockTicker As String, dayStocks As List(Of DayStock))
        Dim detailedOutput As New StringBuilder

        detailedOutput.AppendLine(String.Format("Processing for stock ticker symbol: '{0}'", stockTicker))

        detailedOutput.AppendLine("_______________________________________________________________________________________________")
        detailedOutput.AppendLine("| Day #  | Closing Price | Action  | Quanity  | Cash        |  Shares Owned | Portfolio Value |")
        detailedOutput.AppendLine("|--------|---------------|---------|----------|-------------|---------------|-----------------|")

        Dim portfolio As New Portfolio With
            {
                .Cash = 100000D,
                .SharesOwns = 0
            }

        Challenge.Reset()

        Dim stopWatch As Stopwatch = stopWatch.StartNew

        Dim dayNumber As Integer = 0

        For Each dayStock As DayStock In dayStocks
            dayNumber += 1

            Dim action As DayAction = Challenge.PerformStockAction(dayStock.Clone, portfolio.Clone)

            If action IsNot Nothing AndAlso action.Quantity > 0 Then
                Select Case action.Action
                    Case TradeAction.Buy
                        Dim cost As Decimal = dayStock.Close * action.Quantity
                        If portfolio.Cash - cost >= 0 Then
                            portfolio.Cash -= cost
                            portfolio.SharesOwns += action.Quantity
                        End If
                    Case TradeAction.Sell
                        Dim income As Decimal = dayStock.Close * action.Quantity
                        If action.Quantity <= portfolio.SharesOwns Then
                            portfolio.Cash += income
                            portfolio.SharesOwns -= action.Quantity
                        End If
                End Select
            End If
            detailedOutput.AppendFormat("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |",
                                        dayNumber.ToString("N0").PadLeft(6, " "),
                                        dayStock.Close.ToString("N2").PadLeft(13, " "),
                                        [Enum].GetName(GetType(TradeAction), action.Action).PadLeft(7, " "),
                                        action.Quantity.ToString("N0").PadLeft(8, " "),
                                        portfolio.Cash.ToString("N2").PadLeft(11, " "),
                                        portfolio.SharesOwns.ToString("N0").PadLeft(13, " "),
                                        (portfolio.Cash + (portfolio.SharesOwns * dayStock.Close)).ToString("C").PadLeft(15, " "))
            detailedOutput.AppendLine()

        Next
        

        stopWatch.Stop()

        detailedOutput.AppendLine("|--------|---------------|---------|----------|-------------|---------------|-----------------|")
        detailedOutput.AppendLine()

        portfolio.Cash += portfolio.SharesOwns * dayStocks.Last.Close
        detailedOutput.AppendFormat(" Finished with portfolio value of {0}", portfolio.Cash.ToString("C"))



        Dim score As Long = portfolio.Cash

        Dim challengeResults = New ChallengeResult With
                                 {
                                     .Successful = True,
                                     .Score = score,
                                     .DurationTicks = stopWatch.Elapsed.Ticks,
                                     .ResultMessage = String.Format("Finished with {0}", portfolio.Cash.ToString("C"))
                                }

        If IsTest Then
            challengeResults.TestResults.Add(New FileResult With
                                             {
                                                 .Contents = Encoding.UTF8.GetBytes(detailedOutput.ToString()),
                                                 .Filename = String.Format("{0}.txt", stockTicker)
                                             })
        End If

        ResultsAvailable(challengeResults)



    End Sub

    Private Function GetDailyStockValues(stock As String) As List(Of DayStock)
        Dim dailyStockValues As New List(Of DayStock)

        Dim csvContent As String = ReadResource(stock).Replace(vbCrLf, vbCr).Replace(vbLf, vbCr)

        Dim lines As String() = Split(csvContent, vbCr)

        Dim count As Integer = 0
        For Each line As String In lines
            count += 1
            Dim items As String() = Split(line, ",")

            If items.Count = 7 Then
                Dim dayStockValue As New DayStock

                'dayStockValue.Date = DateTime.Parse(items(0))
                dayStockValue.DayNumber = count
                dayStockValue.Open = Decimal.Parse(items(1))
                dayStockValue.High = Decimal.Parse(items(2))
                dayStockValue.Low = Decimal.Parse(items(3))
                'dayStockValue.Close = Decimal.Parse(items(4))
                dayStockValue.Volume = Integer.Parse(items(5))
                dayStockValue.Close = Decimal.Parse(items(6))

                dailyStockValues.Add(dayStockValue)
            End If
        Next

        dailyStockValues.Reverse()

        Return dailyStockValues
    End Function


    Protected Overrides ReadOnly Property MaxAuthorNotesLength() As Integer
        Get
            Return 0
        End Get
    End Property

    Private Function ReadResource(name As String) As String
        Dim thisAssembly As Assembly = Assembly.GetExecutingAssembly()
        Dim returnValue As String = String.Empty

        Using stream As IO.Stream = thisAssembly.GetManifestResourceStream("OmahaMTG.Challenge.Challenges." & name & ".txt")
            Using sr As New StreamReader(stream)
                returnValue = sr.ReadToEnd
            End Using
        End Using

        Return returnValue
    End Function

End Class
