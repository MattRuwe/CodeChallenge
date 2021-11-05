using System;
using System.Collections.Generic;
using System.Text;
namespace OmahaMTG.Challenge.FrequenciesChallenge
{
    /// <summary>
    /// Represents a collection of Radio Stations for a specific Frequency.
    /// </summary>
    public sealed class FrequencyBand
    {
        private readonly List<Station> stations;

        /// <summary>
        /// Create a new FrequencyBand
        /// </summary>
        public FrequencyBand()
        {
            stations = new List<Station>();
        }
        /// <summary>
        /// Determines if the specified Station will cause conflicts with the Stations already assigned to this FrequencyBand.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool CanAddStation(Station s)
        {
            if (s == null || stations.Contains(s)) { return false; }
            int mx = s.X;
            int my = s.Y;
            int r = s.Range;
            for (int i = stations.Count - 1; i >= 0; i--)
            {
                Station s2 = stations[i];
                int dist = (mx - s2.X) * (mx - s2.X) + (my - s2.Y) * (my - s2.Y);
                if (dist <= (r + s2.Range) * (r + s2.Range)) { return false; }
            }
            return true;
        }
        /// <summary>
        /// Adds the specified Station to this FrequencyBand if it won't cause conflicts.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool AddStation(Station s)
        {
            if (CanAddStation(s))
            {
                stations.Add(s);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes the specified station if it exists
        /// </summary>
        /// <param name="s"></param>
        public void RemoveStation(Station s)
        {
            stations.Remove(s);
        }
        /// <summary>
        /// Removes the station at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveStation(int index)
        {
            stations.RemoveAt(index);
        }
        /// <summary>
        /// Returns a readonly list of Stations associated with this Frequency.
        /// </summary>
        public IEnumerable<Station> Stations
        {
            get { return stations; }
        }
        internal List<Station> GetStations() { return stations; }
        /// <summary>
        /// Determines if this FrequencyBand has conflicts between stations.
        /// </summary>
        /// <returns></returns>
        public bool InConflict()
        {
            for (int j = stations.Count - 1; j >= 0; j--)
            {
                Station s = stations[j];
                int mx = s.X;
                int my = s.Y;
                int r = s.Range;
                for (int i = j - 1; i >= 0; i--)
                {
                    Station s2 = stations[i];
                    int dist = (mx - s2.X) * (mx - s2.X) + (my - s2.Y) * (my - s2.Y);
                    if (dist <= (r + s2.Range) * (r + s2.Range)) { return true; }
                }
            }
            return false;
        }
        /// <summary>
        /// Returns the amount of Stations associated with this Frequency.
        /// </summary>
        public int StationCount { get { return stations.Count; } }
        /// <summary>
        /// Represents this FrequencyBand
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[").Append(stations.Count).Append("]");
            stations.Sort(delegate(Station s1, Station s2) { return s1.StationsInRangeCount < s2.StationsInRangeCount ? 1 : s1.StationsInRangeCount == s2.StationsInRangeCount ? 0 : -1; });
            for (int i = stations.Count - 1; i > 0; i--)
            {
                sb.Append(stations[i].ToString()).Append("|");
            }
            if (stations.Count > 0) { sb.Append(stations[0].ToString()); }
            return sb.ToString();
        }
    }
}