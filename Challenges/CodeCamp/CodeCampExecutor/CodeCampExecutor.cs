using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using OmahaMTG.Challenge.ExecutionCommon;

namespace OmahaMTG.Challenge.Challenges
{
    public class CodeCampExecutor : ChallengeExecutorBase<ICodeCampChallenge>
    {
        protected override void RunChallengeOverride()
        {
            if (IsTest)
            {
                var r = new Random((int)DateTime.Now.Ticks);
                var rooms = r.Next(2, 21);
                var slots = r.Next(3, 21);
                var sessions = rooms * slots;
                var attendees = r.Next(rooms * 20, rooms * 50);

                RunSingleCodeCamp(CodeCampFactory.CreateRandomCodeCamp(attendees, sessions, rooms, slots));
            }
            else
            {
                string[] filenames = {"codecamp1.txt", "codecamp2.txt", "codecamp3.txt"};
                foreach (var filename in filenames)
                {
                    var codeCamp = ReadCodeCamp(filename);
                    RunSingleCodeCamp(codeCamp);
                }
            }
        }

        private void RunSingleCodeCamp(CodeCamp codeCamp)
        {
            var result = new ChallengeResult();

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var schedule = Challenge.ScheduleSessions(codeCamp);
            sw.Stop();

            result.Successful = schedule.IsValid();
            result.DurationTicks = sw.ElapsedTicks;
            result.ResultMessage = "";
            var scheduledSessionScore = (int) Math.Round(100000*((double) schedule.Sessions.Count/(double) codeCamp.Sessions.Count));
            var attendeeConflictPenalty = (int) Math.Round(((double) schedule.GetAttendeeConflictCount()/(double) codeCamp.Attendees.Count)*10000);
            var capacityConflictPenalty = (int) Math.Round(((double) schedule.GetCapacityConflictCount()/(double) codeCamp.Attendees.Count)*5000);
            var timePenalty = (int) (TimeSpan.FromTicks(result.DurationTicks).TotalMilliseconds);
            result.Score = scheduledSessionScore - attendeeConflictPenalty - capacityConflictPenalty - timePenalty;

            if (result.Score < 0)
                result.Score = 0;

            ResultsAvailable(result);
        }

        protected override int MaxAuthorNotesLength
        {
            get { return 5000; }
        }

        private CodeCamp ReadCodeCamp(string filename)
        {
            CodeCamp codeCamp;
            using (Stream stream = Assembly.GetAssembly(typeof(CodeCampExecutor)).GetManifestResourceStream("OmahaMTG.Challenge.Challenges." + filename))
            {
                if (stream == null)
                    return null;
                var ser = new DataContractJsonSerializer(typeof(CodeCamp));
                codeCamp = (CodeCamp)ser.ReadObject(stream);
            }
            return codeCamp;
        }
    }
}