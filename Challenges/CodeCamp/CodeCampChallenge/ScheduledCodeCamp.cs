using System;
using System.Collections.Generic;
using System.Linq;

namespace OmahaMTG.Challenge.Challenges
{
    public class ScheduledCodeCamp
    {
        private readonly CodeCamp _codeCamp;

        public ScheduledCodeCamp(CodeCamp codeCamp)
        {
            _codeCamp = codeCamp;
            Sessions = new List<ScheduledSession>();
        }

        public List<ScheduledSession> Sessions { get; set; }

        public void ScheduleSession(ScheduledSession session, bool overwrite = false)
        {
            if(session.Room == null || session.TimeSlot == null)
                throw new SessionNotScheduledException();

            var previouslyScheduledSession = GetScheduledSession(session.Room, session.TimeSlot);
            if (previouslyScheduledSession != null && !overwrite)
            {
                throw new RoomConflictException();
            }

            foreach (var speaker in _codeCamp.Sessions.Single(x=> x.Equals(session.Session)).Speakers)
            {
                var speakerSessions = Sessions.Where(x => _codeCamp.Sessions.Where(s => s.Speakers.Contains(speaker)).Contains(x.Session));
                var scheduledTimeSlots = speakerSessions.Select(x => x.TimeSlot);
                if(scheduledTimeSlots.Contains(session.TimeSlot))
                    throw new SpeakerConflictException();
            }

            if (previouslyScheduledSession != null)
                Sessions.Remove(previouslyScheduledSession);
            Sessions.Add(session);
        }

        public void UnScheduleSession(Session session)
        {
            var scheduledSession = Sessions.SingleOrDefault(x => x.Session.Equals(session));
            if (scheduledSession != null)
                Sessions.Remove(scheduledSession);
        }

        public void UnScheduleSession(Room room, TimeSlot timeSlot)
        {
            var scheduledSession = GetScheduledSession(room, timeSlot);
            if (scheduledSession != null)
                Sessions.Remove(scheduledSession);
        }

        public ScheduledSession GetScheduledSession(Room room, TimeSlot timeSlot)
        {
            return Sessions.SingleOrDefault(x => x.Room.Equals(room) && x.TimeSlot.Equals(timeSlot));
        }

        public int GetAttendeeConflictCount()
        {
            int conflicts = 0;
            foreach(var attendee in _codeCamp.Attendees)
            {
                var sessionCount = attendee.Sessions.Count;
                var timeSlotCount = Sessions.Where(x => attendee.Sessions.Contains(x.Session)).Select(x => x.TimeSlot).Distinct().Count();
                conflicts += (sessionCount - timeSlotCount);
            }
            return conflicts;
        }


        public int GetCapacityConflictCount()
        {
            var count = 0;
            foreach (var session in Sessions)
            {
                var attendees = _codeCamp.Attendees.Count(x => x.Sessions.Contains(session.Session));
                count += Math.Max(0, attendees - session.Room.Capacity);
            }
            return count;
        }


        public bool IsValid()
        {
            foreach (var speaker in _codeCamp.Speakers)
            {
                var speakerSessions = _codeCamp.Sessions.Where(x => x.Speakers.Contains(speaker));
                var scheduledSessions = Sessions.Where(x => speakerSessions.Contains(x.Session));
                var timeSlots = scheduledSessions.Select(x => x.TimeSlot).Distinct();
                if (scheduledSessions.Count() - timeSlots.Count() > 0)
                    return false;
            }

            foreach(var session in Sessions)
            {
                if (Sessions.Count(x => x.Session == session.Session) > 1)
                    return false;
                if (Sessions.Count(x => x.Room.Equals(session.Room) && x.TimeSlot.Equals(session.TimeSlot)) > 1)
                    return false;
            }

            return true;
        }

        public int ScheduledSessions()
        {
            return Sessions.Count();
        }
    }
}