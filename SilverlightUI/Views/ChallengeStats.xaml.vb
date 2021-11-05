Imports Telerik.Windows.Controls.Charting
Imports System.Windows.Threading

Partial Public Class ChallengeStats
    Inherits ChildWindow

    Private _refreshTimer As DispatcherTimer

    Public Shared ChallengeStatisticsProperty As DependencyProperty = DependencyProperty.Register("ChallengeStatistics", GetType(ChallengeStatistics), GetType(ChallengeStats), Nothing)

    Public Property ChallengeStatistics As ChallengeStatistics
        Get
            Return CType(GetValue(ChallengeStatisticsProperty), ChallengeStatistics)
        End Get
        Set(value As ChallengeStatistics)
            SetValue(ChallengeStatisticsProperty, value)
        End Set
    End Property


    Public Sub New(challengeId As Integer)
        InitializeComponent()
        challengeIdQueryParameter.Value = challengeId
    End Sub

    'Public Shared ReadOnly ChallengeIDProperty As DependencyProperty = DependencyProperty.Register("ChallengeID", GetType(Integer), GetType(ChallengeStats), Nothing)

    'Public Property ChallengeID As Integer
    '    Get
    '        Return CType(GetValue(ChallengeIDProperty), Integer)
    '    End Get
    '    Set(value As Integer)
    '        SetValue(ChallengeIDProperty, value)
    '        ddsChallengeStats.Load()
    '    End Set
    'End Property

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub ddsChallengeStats_LoadedData(sender As System.Object, e As System.Windows.Controls.LoadedDataEventArgs) Handles ddsChallengeStats.LoadedData
        If _refreshTimer Is Nothing Then
            _refreshTimer = New DispatcherTimer()
            _refreshTimer.Interval = TimeSpan.FromMinutes(1)
            AddHandler _refreshTimer.Tick, Sub(sender2 As Object, e2 As EventArgs)
                                               ddsChallengeStats.Load()
                                           End Sub
            _refreshTimer.Start()
        End If

        If e.Entities.Count > 0 Then
            ChallengeStatistics = CType(e.Entities.First, ChallengeStatistics)
            DataContext = ChallengeStatistics

            With chart.DefaultView.ChartTitle
                .Content = ChallengeStatistics.ChallengeName
                .HorizontalAlignment = HorizontalAlignment.Center
            End With

            With chart.DefaultView.ChartArea.AxisX
                .IsDateTime = True
                .LayoutMode = AxisLayoutMode.Inside
                .LabelRotationAngle = 45
                .DefaultLabelFormat = "dd-MMM"
            End With

            'With chart.DefaultView.ChartArea.ZoomScrollSettingsX
            '    .MinZoomRange = 0.1
            '    .RangeStart = 0.1
            '    .RangeEnd = 0.9
            '    .ScrollMode = ScrollMode.ScrollAndZoom
            'End With

            Dim userMaxScore As Long = 0

            'chart.DefaultView.ChartArea.DataSeries.Clear()
            For Each stat As UserChallengeStatistics In ChallengeStatistics.UserStatistics
                Dim series As DataSeries = Nothing

                For Each loopSeries In chart.DefaultView.ChartArea.DataSeries
                    If loopSeries.LegendLabel = stat.Username Then
                        series = loopSeries
                        series.Clear()
                        Exit For
                    End If
                Next

                If series Is Nothing Then
                    series = New DataSeries
                    series.LegendLabel = stat.Username
                    series.Definition = New LineSeriesDefinition()
                    chart.DefaultView.ChartArea.DataSeries.Add(series)
                End If

                With series.Definition
                    .ShowItemLabels = False
                    .ShowItemToolTips = True
                End With

                For Each score As UserScore In stat.Scores
                    If score.Score > userMaxScore Then
                        series.Add(New DataPoint(score.DateAdded.ToOADate, score.Score))
                        userMaxScore = score.Score
                    End If
                Next
                userMaxScore = 0
            Next
        End If

    End Sub

    Private Sub ddsChallengeStats_LoadingData(sender As Object, e As System.Windows.Controls.LoadingDataEventArgs) Handles ddsChallengeStats.LoadingData
        e.LoadBehavior = ServiceModel.DomainServices.Client.LoadBehavior.RefreshCurrent
    End Sub
End Class