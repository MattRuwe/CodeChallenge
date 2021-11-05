Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.Challenges

Public Class StockTrader
    Implements IStockTraderChallenge

    Public ReadOnly Property AuthorNotes() As String Implements IChallenge.AuthorNotes
        Get
            'Author notes are not allowed in this challenge
            Return String.Empty
        End Get
    End Property

    'Provides a place to store the prices that we've receive
    Private _history As List(Of Decimal)

    Public Sub Reset() Implements IStockTraderChallenge.Reset
        'Do any setup needed here - your entry is not reconstructed between runs
        _history = New List(Of Decimal)
    End Sub

    Public Function PerformStockAction(stock As DayStock, portfolio As Portfolio) As DayAction Implements IStockTraderChallenge.PerformStockAction
        'This function is called once for every day of the stock
        'Store any information you need between calls as a class level variable (the constructor for this class is only called once)
        Dim returnValue As New DayAction
        If _history.Count > 0 Then
            If _history.Last > stock.Close Then
                'The price is going down, so it's time to sell some stock
                returnValue.Action = TradeAction.Sell
                returnValue.Quantity = portfolio.SharesOwns * 0.5
            ElseIf _history.Last < stock.Close Then
                'The price is going up, so it's time to buy some stock
                returnValue.Action = TradeAction.Buy
                returnValue.Quantity = (portfolio.Cash * 0.1) / stock.Close
            End If
        End If

        _history.Add(stock.Close)

        Return returnValue
    End Function
End Class
