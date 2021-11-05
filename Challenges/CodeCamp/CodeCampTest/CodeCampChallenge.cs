using System;
using System.Linq;
using OmahaMTG.Challenge.Challenges;

namespace CodeCampTest
{
    public class CodeCampChallenge : ICodeCampChallenge
    {
        public string AuthorNotes
        {
            get { return "This is a test."; }
        }

        public ScheduledCodeCamp ScheduleSessions(CodeCamp codeCamp)
        {
            var schedule = new ScheduledCodeCamp(codeCamp);

            int idx = 0;

            foreach(var room in codeCamp.Rooms)
                foreach(var timeSlot in codeCamp.TimeSlots)
                {
                    if (idx == codeCamp.Sessions.Count)
                        break;
                    var session = codeCamp.Sessions[idx];

                    try
                    {
                        schedule.ScheduleSession(new ScheduledSession {Room = room, TimeSlot = timeSlot, Session = session});
                    }
                    catch (SpeakerConflictException ex)
                    {
                        continue;
                    }
                    finally
                    {
                        idx++;
                    }
                }
            return schedule;
        }
    }
}
