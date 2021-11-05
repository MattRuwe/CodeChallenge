using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Drawing;
namespace OmahaMTG.Challenge.FrequenciesChallenge
{
    /// <summary>
    /// Represents a radio Station at a specific location (x,y) and a broadcast range.
    /// </summary>
    public sealed class Station
    {
        private readonly int x, y, range;
        private readonly List<Station> stationsInRange;

        /// <summary>
        /// Station
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="range"></param>
        /// <param name="stationsInRange"></param>
        public Station(int x, int y, int range, List<Station> stationsInRange)
        {
            this.stationsInRange = stationsInRange;
            this.x = x;
            this.y = y;
            this.range = range;
        }
        /// <summary>
        /// Returns the X coordinate of this Station
        /// </summary>
        public int X { get { return x; } }
        /// <summary>
        /// Returns the Y coordinate of this Station
        /// </summary>
        public int Y { get { return y; } }
        /// <summary>
        /// Returns the Range of this Station
        /// </summary>
        public int Range { get { return range; } }
        /// <summary>
        /// Returns a clone of the specified Stations.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Station> Clone(IEnumerable<Station> stations)
        {
            List<Station> retVal = new List<Station>();
            foreach (Station s in stations)
            {
                retVal.Add(new Station(s.x, s.y, s.range, new List<Station>()));
            }
            int amount = retVal.Count;
            for (int i = 0; i < amount; i++)
            {
                Station s = retVal[i];
                int mx = s.x;
                int my = s.y;
                int r = s.range;
                List<Station> list = s.stationsInRange;
                for (int j = i + 1; j < amount; j++)
                {
                    Station s2 = retVal[j];
                    int dist = (mx - s2.x) * (mx - s2.x) + (my - s2.y) * (my - s2.y);
                    if (dist <= (r + s2.range) * (r + s2.range))
                    {
                        list.Add(s2);
                        s2.stationsInRange.Add(s);
                    }
                }
            }
            return retVal;
        }
        /// <summary>
        /// Returns a list of nearby radio stations that are in the same broadcast range.
        /// </summary>
        public IEnumerable<Station> StationsInRange
        {
            get { return stationsInRange; }
        }
        /// <summary>
        /// Returns the count of stations in range of this station.
        /// </summary>
        public int StationsInRangeCount { get { return stationsInRange.Count; } }
        /// <summary>
        /// Returns a representation of this radio station.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + x + "," + y + ")[R:" + range + ")[C:" + stationsInRange.Count + "]";
        }
        /// <summary>
        /// Returns true if the two Stations are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Station) || obj.GetHashCode() != (x | (y << 16))) { return false; }
            Station s = (Station)obj;
            if (s.range != range || s.stationsInRange.Count != stationsInRange.Count) { return false; }
            for (int i = stationsInRange.Count - 1; i >= 0; i--)
            {
                Station a = stationsInRange[i];
                Station b = s.stationsInRange[i];
                if (a == null || b == null || a.x != b.x || a.y != b.y || a.range != b.range || a.StationsInRangeCount != b.StationsInRangeCount) { return false; }
            }
            return true;
        }
        /// <summary>
        /// Returns a unique int to represent this station.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (x | (y << 16));
        }
        /// <summary>
        /// Creates a PNG image of the specified radio stations.
        /// </summary>
        /// <param name="filePath">The file path of the image to be created. ie) "C:\Stations.png".</param>
        /// <param name="stations">The list of stations to be drawn.</param>
        /// <param name="drawRange">Boolean to specify whether or not to draw the broadcast ranges of the stations.</param>
        /// <param name="drawConnections">Boolean to specify whether or not to draw lines connecting stations that are in range of each other.</param>
        public static void CreateImage(string filePath, IEnumerable<Station> stations, bool drawRange = true, bool drawConnections = true)
        {
            if (stations == null) { throw new ArgumentNullException("Stations cannot be null."); }
            if (string.IsNullOrEmpty(filePath)) { throw new ArgumentNullException("A FilePath must be specified."); }
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            if (File.Exists(filePath) || Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                File.WriteAllBytes(filePath, CreateImage(stations, drawRange, drawConnections));
            }
        }
        public static byte[] CreateImage(IEnumerable<Station> stations, bool drawRange = true, bool drawConnections = true)
        {
            List<Station> s = stations.ToList<Station>();
            int amount = s.Count;
            int maxX = 0, maxY = 0;
            int minX = Int32.MaxValue, minY = Int32.MaxValue;
            HashSet<Station> dots = new HashSet<Station>();
            HashSet<Station> ranges = new HashSet<Station>();
            for (int i = 0; i < amount; i++)
            {
                if (s[i].x > maxX) { maxX = s[i].x; }
                if (s[i].x < minX) { minX = s[i].x; }
                if (s[i].y > maxY) { maxY = s[i].y; }
                if (s[i].y < minY) { minY = s[i].y; }
                List<Station> list = s[i].stationsInRange;
                for (int j = list.Count - 1; j >= 0; j--)
                {
                    if (list[j].x > maxX) { maxX = list[j].x; }
                    if (list[j].x < minX) { minX = list[j].x; }
                    if (list[j].y > maxY) { maxY = list[j].y; }
                    if (list[j].y < minY) { minY = list[j].y; }
                }
            }
            using (Bitmap bmp = new Bitmap(maxX - minX + 20, maxY - minY + 20, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.Clear(Color.White);
                    SolidBrush p = new SolidBrush(Color.FromArgb(40, 0, 0, 0));
                    for (int i = 0; i < amount; i++)
                    {
                        if (!dots.Contains(s[i]))
                        {
                            g.FillEllipse(Brushes.Black, s[i].x - minX + 6, s[i].y - minY + 6, 8, 8);
                            dots.Add(s[i]);
                        }
                        List<Station> list = s[i].stationsInRange;
                        for (int j = list.Count - 1; j >= 0; j--)
                        {
                            if (!dots.Contains(list[j]))
                            {
                                g.FillEllipse(Brushes.Black, list[j].x - minX + 6, list[j].y - minY + 6, 8, 8);
                                dots.Add(list[j]);
                            }
                            if (drawConnections) { g.DrawLine(Pens.Black, list[j].x - minX + 10, list[j].y - minY + 10, s[i].x - minX + 10, s[i].y - minY + 10); }
                            if (!ranges.Contains(list[j]))
                            {
                                if (drawRange) { g.FillEllipse(p, list[j].x - list[j].range - minX + 10, list[j].y - list[j].range - minY + 10, list[j].range * 2, list[j].range * 2); }
                                ranges.Add(list[j]);
                            }
                        }
                        if (!ranges.Contains(s[i]))
                        {
                            if (drawRange) { g.FillEllipse(p, s[i].x - s[i].range - minX + 10, s[i].y - s[i].range - minY + 10, s[i].range * 2, s[i].range * 2); }
                            ranges.Add(s[i]);
                        }
                    }
                }
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Generates a new list of radio stations with a specified distribution.
        /// </summary>
        /// <param name="amount">Amount of radio stations to create.</param>
        /// <param name="minRange">Minimum range a station can have.</param>
        /// <param name="maxRange">Maximum range a station can have.</param>
        /// <param name="minDistanceApart">Mimimum distance two stations can be next to each other.</param>
        /// <param name="sparseness">Determines how sparsely populated the radio stations will be.</param>
        /// <returns></returns>
        public static IEnumerable<Station> CreateStations(int amount, int minRange, int maxRange, int minDistanceApart, int sparseness)
        {
            if (amount < 2) { throw new ArgumentOutOfRangeException("Amount must be greater than 2."); }
            if (minDistanceApart < 5) { throw new ArgumentOutOfRangeException("MinDistanceApart must be greater than 4."); }
            if (minRange < 5) { throw new ArgumentOutOfRangeException("MinRange must be greater than 4."); }
            if (maxRange >= 512) { throw new ArgumentOutOfRangeException("MaxRange must be less than 512."); }
            if (maxRange < minRange) { throw new ArgumentOutOfRangeException("MaxRange must be greater than or equal to MinRange."); }
            if (sparseness < 0) { throw new ArgumentOutOfRangeException("Sparseness must be greater than 0."); }
            Random rnd = new Random();
            int max = (int)Math.Ceiling(Math.Sqrt(amount)) * (minDistanceApart + sparseness + 5);
            if (max >= 65536) { throw new Exception("Combination of Amount, MinDistanceApart, and Sparseness is too large. Please reduce the values."); }
            List<Station> stations = new List<Station>();
            for (int i = 0; i < amount; i++)
            {
                int mx = rnd.Next(0, max);
                int my = rnd.Next(0, max);
                int r = rnd.Next(minRange, maxRange + 1);
                while (stations.Exists(delegate(Station s) { return (mx - s.x) * (mx - s.x) + (my - s.y) * (my - s.y) < minDistanceApart * minDistanceApart; }))
                {
                    mx = rnd.Next(0, max);
                    my = rnd.Next(0, max);
                }
                stations.Add(new Station(mx, my, r, new List<Station>()));
            }

            for (int i = 0; i < amount; i++)
            {
                Station s = stations[i];
                int mx = s.x;
                int my = s.y;
                int r = s.range;
                List<Station> list = s.stationsInRange;
                for (int j = i + 1; j < amount; j++)
                {
                    Station s2 = stations[j];
                    int distp = (mx - s2.x) * (mx - s2.x) + (my - s2.y) * (my - s2.y);
                    if (distp <= (r + s2.range) * (r + s2.range))
                    {
                        list.Add(s2);
                        s2.stationsInRange.Add(s);
                    }
                }
            }
            return stations;
        }
    }
}