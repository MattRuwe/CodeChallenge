using System;
using System.Collections.Generic;
using System.Text;
using OmahaMTG.Challenge.ChallengeCommon;
using OmahaMTG.Challenge.ExecutionCommon;
using OmahaMTG.Challenge.Challenges;
using OmahaMTG.Challenge.SolitaireChallenge;
namespace OmahaMTG.Challenge.Challenges
{
    public class SolitaireExecutor : ChallengeExecutorBase<ISolitaireChallenge>
    {
        protected override int MaxAuthorNotesLength
        {
            get { return 0; }
        }

        protected override void RunChallengeOverride()
        {
            string[] cardsets = {"044112092034103111093074114031024042091084064053052021101062073121063113011054083014122102043013132033032041051082023134012104061081124131133094123071022072",
                                 "093051032092074131014103064031091071104013041111053102112132063061072054052133023113082043134033044114042122124101024084021123081022083034062012121073011094",
                                 "122102054114092084012023091124133113032022123082103101034052044011131083112051134104061064033014094021111063053062031072041024121081043073013093074071042132",
                                 "093011091064043131071122084051074092061031062012081103032054134013014073133072041113052053121132044082042124111023083101033021123034112024094022063102104114",
                                 "081054022072134033082024052064053012061013042093084124092122062031083121113023043074051114091014103044131063041102101133011111071073034123104112021132032094",
                                 "012054062044022101102122074071041011133024043013093111082034114042131103061051063053094092072084132091112023113052032031134014123064104124121021081083033073",
                                 "064083042011093052131072024091081082014041101071094073062123122114031061133034104103102043121134021054132033084013044063074032111113051124092023053022112012",
                                 "131101034061073072044071082043092042094132014081051124083113121052063054012103062064032111134011074093053122102033013084031114091112104021133024022041123023",
                                 "102072022064073114071042012053044133021051093124081054013112084101123132091134113041074094131104031023014061122062011103083082063121032111024092033052043034",
                                 "112101053072123052051081023111131032124063062041044122054021024093103033071073082121022094083074133102104084114134042113011061031013091064034014043012132092"};
            int[] seq = new int[cardsets.Length];
            for (int i = 0; i < cardsets.Length; i++)
            {
                seq[i] = i;
            }
            Rand rnd = new Rand();
            for (int i = 0; i < 1000; i++)
            {
                int r1 = rnd.Next % cardsets.Length;
                int r2 = rnd.Next % cardsets.Length;
                string temp = cardsets[r1];
                cardsets[r1] = cardsets[r2];
                cardsets[r2] = temp;
            }

            Solitaire s = new Solitaire();
            ChallengeResult[] res = new ChallengeResult[cardsets.Length];
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            for (int i = 0; i < cardsets.Length; i++)
            {
                res[i] = new ChallengeResult();
                res[i].Successful = true;
                s.CardSet = cardsets[seq[i]];
                sw.Reset();
                sw.Start();
                try
                {
                    IEnumerable<Move> ien = Challenge.Solve(s.Clone());
                    if (ien == null)
                    {
                        res[i].ResultMessage = "Solitaire deal was not solved.";
                        res[i].Score = 0;
                        res[i].Successful = false;
                    }
                    else
                    {
                        IEnumerator<Move> en = ien.GetEnumerator();
                        en.MoveNext();
                        while (en.Current != null)
                        {
                            if (!s.MakeMove(en.Current))
                            {
                                res[i].DisplayError = "Invalid move made.";
                                break;
                            }
                            if (s.IsWon || s.TotalMovesMade > 1000)
                            {
                                break;
                            }
                            en.MoveNext();
                        }
                        res[i].Successful = s.IsWon;
                        if (s.IsWon)
                        {
                            res[i].Score = s.TotalMovesMade - 97;
                            if (s.TotalMovesMade > 256)
                            {
                                res[i].ResultMessage = "Solitaire deal was solved.";
                            }
                            else
                            {
                                res[i].ResultMessage = "Solitaire deal was solved in " + s.TotalMovesMade + " moves.";
                            }
                        }
                        else
                        {
                            res[i].ResultMessage = "Solitaire deal was not solved.";
                            res[i].Score = 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e != null && e.Data.Contains("Custom"))
                    {
                        res[i].DisplayError = e.Message;
                        res[i].ResultMessage = "Solitaire deal was not solved.";
                        res[i].DetailedError = "";
                    }
                    else
                    {
                        res[i].DisplayError = "A runtime error occured.";
                        res[i].ResultMessage = "An error occurred while the user challenge was running.";
                        res[i].DetailedError = e.ToString();
                    }
                    res[i].Successful = false;
                    res[i].Score = 0;
                }
                sw.Stop();
                res[i].DurationTicks = sw.Elapsed.Ticks;
            }
            long score = 100000;
            int dur = 0;
            int moveVal = 630 / cardsets.Length;
            long maxVal = score / cardsets.Length;
            for (int i = 0; i < cardsets.Length; i++)
            {
                dur += (int)(res[i].DurationTicks / 1000L);
                if (res[i].Successful)
                {
                    long temp = res[i].Score * moveVal;
                    if (temp > maxVal) { temp = maxVal; }
                    score -= temp;
                }
                else
                {
                    score -= maxVal;
                }
            }
            score -= (dur / 10000);
            if (score < 0) { score = 0; }
            for (int i = 0; i < cardsets.Length; i++)
            {
                res[i].Score = score;
                ResultsAvailable(res[i]);
            }
        }
    }
}
