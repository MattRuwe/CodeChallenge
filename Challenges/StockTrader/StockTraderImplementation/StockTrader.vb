Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.Challenges

Public Class StockTrader
    Implements IStockTraderChallenge

    Private Const MINIMUM_HISTORY = 10

    Public ReadOnly Property AuthorNotes() As String Implements IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Public Sub Reset() Implements IStockTraderChallenge.Reset
        _trend = 0
        _historicalPrices = New List(Of Decimal)
        _movingAverage = New List(Of Decimal)
    End Sub

    Private _trend As Integer
    Private _historicalPrices As List(Of Decimal)
    Private _movingAverage As List(Of Decimal)

    Public Function PerformStockAction(stock As DayStock, portfolio As Portfolio) As DayAction Implements IStockTraderChallenge.PerformStockAction
        Dim returnValue = New DayAction

        _historicalPrices.Add(stock.Close)
        _movingAverage.Add(_historicalPrices.Average)

        If _historicalPrices.Count > MINIMUM_HISTORY Then
            Dim trend As Integer
            Dim previousValue As Decimal
            For Each price As Decimal In _movingAverage.Skip(_movingAverage.Count - MINIMUM_HISTORY)
                If previousValue < price Then
                    trend += 1
                ElseIf previousValue > price Then
                    trend -= 1
                End If
                previousValue = price
            Next

            If trend > 0 Then
                returnValue.Action = TradeAction.Buy
                returnValue.Quantity = (portfolio.Cash * 0.1) / stock.Close
            Else
                returnValue.Action = TradeAction.Sell
                returnValue.Quantity = portfolio.SharesOwns * 0.5
            End If
        End If


        Return returnValue
    End Function

End Class
