Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.FrequenciesChallenge
Public Class FrequencyAssigner
    Implements IFrequenciesChallenge

    Public Function AssignStations(ByVal stations As IEnumerable(Of Station)) As IEnumerable(Of FrequencyBand) Implements IFrequenciesChallenge.AssignStations
        'If you want to see a pictorial representation of the layout of the stations.
        'Station.CreateImage("C:\RadioStations.png", stations, True, True)

        Dim frequencies As New List(Of FrequencyBand)
        'Assign one frequency per station.
        For Each s As Station In stations
            Dim fb As New FrequencyBand()
            If fb.AddStation(s) Then
                'Was able to add station s to this frequency band since it doesn't
                'conflict with any other stations already added (which are none in this example).
                frequencies.Add(fb)
            End If
        Next

        Return frequencies
    End Function

    Public ReadOnly Property AuthorNotes() As String Implements IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property
End Class