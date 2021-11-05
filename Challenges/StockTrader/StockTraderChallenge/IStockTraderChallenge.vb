Imports OmahaMTG.Challenge.ChallengeCommon
  
Public Interface IStockTraderChallenge
Inherits IChallenge
    Function PerformStockAction(stock As DayStock, portfolio As Portfolio) As DayAction
    Sub Reset()
End Interface
