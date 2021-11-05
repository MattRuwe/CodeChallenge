using System;
using System.Collections.Generic;
using System.Linq;
using OmahaMTG.Challenge.FrequenciesChallenge;
namespace FrequenciesImplementation
{
    public class FrequencyAssigner : IFrequenciesChallenge
    {
        public IEnumerable<FrequencyBand> AssignStations(IEnumerable<Station> stations)
        {
            //If you want to see a pictorial representation of the layout of the stations.
            //Station.CreateImage("C:\\RadioStations.png", stations, true, true);

            List<FrequencyBand> frequencies = new List<FrequencyBand>();
            //Assign one frequency per station.
            foreach (Station s in stations)
            {
                FrequencyBand fb = new FrequencyBand();
                if (fb.AddStation(s))
                {
                    //Was able to add station s to this frequency band since it doesn't
                    //conflict with any other stations already added (which are none in this example).
                    frequencies.Add(fb);
                }
            }

            return frequencies;
        }

        public string AuthorNotes
        {
            get { return string.Empty; }
        }
    }
}