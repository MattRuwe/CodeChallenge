using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OmahaMTG.Challenge.ChallengeCommon;
using OmahaMTG.Challenge.ExecutionCommon;
using OmahaMTG.Challenge.Challenges;
using OmahaMTG.Challenge.FrequenciesChallenge;
using System.Diagnostics;
namespace OmahaMTG.Challenge.Challenges
{
    public class FrequenciesExecutor : ChallengeExecutorBase<IFrequenciesChallenge>
    {
        protected override int MaxAuthorNotesLength
        {
            get { return 0; }
        }

        protected override void RunChallengeOverride()
        {
            int[] ind = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] seeds = { 51766516, 78184136, 98654321, 6855423, 59821316, 2987413, 88654321, 65465, 468537, 1275435 };
            int[] minR = { 20, 30, 40, 50, 40, 30, 20, 30, 40, 50 };
            int[] maxR = { 150, 180, 210, 240, 270, 300, 270, 240, 210, 180 };
            int[] minD = { 20, 30, 40, 30, 20, 40, 30, 30, 40, 20 };
            int[] spr = { 30, 30, 30, 30, 30, 30, 30, 30, 30, 20 };
            Rand rnd = new Rand();
            for (int i = 0; i < 1000; i++)
            {
                int r1 = rnd.Next % seeds.Length;
                int r2 = rnd.Next % seeds.Length;
                int temp = ind[r1];
                ind[r1] = ind[r2];
                ind[r2] = temp;
            }

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < seeds.Length; i++)
            {
                ChallengeResult res = new ChallengeResult();
                string detail = string.Empty;
                res.Successful = false;
                try
                {
                    sw.Reset();
                    sw.Start();
                    List<Station> stations = null;
                    if (IsTest)
                    {
                        stations = (List<Station>)CreateStations(10 + (rnd.Next & 7), 20, 150, 30, 50, seeds[ind[i]]);
                        FileResult f = new FileResult()
                        {
                            Contents = Station.CreateImage(stations),
                            Filename = "MapOfRadioStationsAndRanges.png"
                        };
                        res.TestResults.Add(f);
                    }
                    else
                    {
                        stations = (List<Station>)CreateStations(3000, minR[ind[i]], maxR[ind[i]], minD[ind[i]], spr[ind[i]], seeds[ind[i]]);
                    }
                    int maxFreq = 0;
                    for (int j = stations.Count - 1; j >= 0; j--)
                    {
                        if (stations[j].StationsInRangeCount > maxFreq)
                        {
                            maxFreq = stations[j].StationsInRangeCount;
                        }
                    }
                    maxFreq++;
                    IEnumerable<FrequencyBand> ien = Challenge.AssignStations(Station.Clone(stations));
                    if (ien == null)
                    {
                        res.ResultMessage = "No frequencies returned.";
                        res.Score = 0;
                    }
                    else
                    {
                        List<FrequencyBand> f = ien.ToList<FrequencyBand>();
                        HashSet<Station> unique = new HashSet<Station>();
                        int count = 0;
                        for (int j = f.Count - 1; j >= 0; j--)
                        {
                            List<Station> list = f[j].Stations.ToList<Station>();
                            count += list.Count;
                            for (int k = list.Count - 1; k >= 0; k--)
                            {
                                if (unique.Contains(list[k]))
                                {
                                    res.ResultMessage = "A station has been assigned multiple frequencies.";
                                    detail = res.ResultMessage;
                                    throw new Exception();
                                }
                                else if (!stations.Contains(list[k]))
                                {
                                    res.ResultMessage = "An unknown station is referenced in a frequency band.";
                                    detail = res.ResultMessage;
                                    throw new Exception();
                                }
                                unique.Add(list[k]);
                            }
                            if (f[j].InConflict())
                            {
                                res.ResultMessage = "A frequency band has stations that conflict with one another.";
                                detail = res.ResultMessage;
                                throw new Exception();
                            }
                        }

                        if (count == stations.Count)
                        {
                            res.ResultMessage = "All stations assigned successfully.";
                            detail = "All " + count + " stations have been assigned successfully a total of " + f.Count + " different frequencies.";
                            res.Score = maxFreq * count * 10 / f.Count;
                            res.Successful = true;
                        }
                        else if (count < stations.Count)
                        {
                            res.ResultMessage = "Not all stations have frequency assignments.";
                            detail = "There are " + (stations.Count - count) + " stations missing frequency assignments out of " + stations.Count + ".";
                            throw new Exception();
                        }
                    }
                    sw.Stop();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    if (string.IsNullOrEmpty(res.ResultMessage))
                    {
                        res.ResultMessage = "An error occured.";
                        detail = e.ToString();
                    }
                    res.Score = 0;
                    sw.Stop();
                }
                res.DurationTicks = sw.Elapsed.Ticks;
                res.Score -= (int)(res.DurationTicks / 100000L);
                res.Score = res.Score < 0 ? 0 : res.Score;

                if (IsTest)
                {
                    FileResult f = new FileResult()
                    {
                        Contents = Encoding.UTF8.GetBytes(detail),
                        Filename = "FrequenciesOutput.txt"
                    };
                    res.TestResults.Add(f);
                }
                ResultsAvailable(res);
            }
        }

        public static IEnumerable<Station> CreateStations(int amount, int minRange, int maxRange, int minDistanceApart, int sparseness, int seed)
        {
            if (amount < 2) { throw new ArgumentOutOfRangeException("Amount must be greater than 2."); }
            if (minDistanceApart < 5) { throw new ArgumentOutOfRangeException("MinDistanceApart must be greater than 4."); }
            if (minRange < 5) { throw new ArgumentOutOfRangeException("MinRange must be greater than 4."); }
            if (maxRange >= 512) { throw new ArgumentOutOfRangeException("MaxRange must be less than 512."); }
            if (maxRange < minRange) { throw new ArgumentOutOfRangeException("MaxRange must be greater than or equal to MinRange."); }
            if (sparseness < 0) { throw new ArgumentOutOfRangeException("Sparseness must be greater than 0."); }
            Rand rnd = new Rand(seed);
            int max = (int)Math.Ceiling(Math.Sqrt(amount)) * (minDistanceApart + sparseness + 5);
            if (max >= 65536) { throw new Exception("Combination of Amount, MinDistanceApart, and Sparseness is too large. Please reduce the values."); }
            List<Station> stations = new List<Station>();
            for (int i = 0; i < amount; i++)
            {
                int mx = rnd.Next % max;
                int my = rnd.Next % max;
                int r = (rnd.Next % (maxRange + 1 - minRange)) + minRange;
                while (stations.Exists(delegate(Station s) { return (mx - s.X) * (mx - s.X) + (my - s.Y) * (my - s.Y) < minDistanceApart * minDistanceApart; }))
                {
                    mx = rnd.Next % max;
                    my = rnd.Next % max;
                }
                stations.Add(new Station(mx, my, r, new List<Station>()));
            }

            for (int i = 0; i < amount; i++)
            {
                Station s = stations[i];
                int mx = s.X;
                int my = s.Y;
                int r = s.Range;
                List<Station> list = (List<Station>)s.StationsInRange;
                for (int j = i + 1; j < amount; j++)
                {
                    Station s2 = stations[j];
                    int distp = (mx - s2.X) * (mx - s2.X) + (my - s2.Y) * (my - s2.Y);
                    if (distp <= (r + s2.Range) * (r + s2.Range))
                    {
                        list.Add(s2);
                        ((List<Station>)s2.StationsInRange).Add(s);
                    }
                }
            }
            return stations;
        }
    }
}
