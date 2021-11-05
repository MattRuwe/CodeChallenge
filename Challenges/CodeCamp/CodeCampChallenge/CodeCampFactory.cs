using System;
using System.Collections.Generic;

namespace OmahaMTG.Challenge.Challenges
{
    public static class CodeCampFactory
    {
        public static CodeCamp CreateRandomCodeCamp(int attendeeCount, int sessionCount, int roomCount, int timeSlotCount)
        {
            var attendees = new List<Attendee>();
            var sessions = new List<Session>(sessionCount);
            var speakers = new List<Speaker>();
            var rooms = new List<Room>();
            var timeSlots = new List<TimeSlot>();

            var rand = new Random((int)DateTime.Now.Ticks);

            int avgRoomCapacity = attendeeCount/roomCount;
            for (int i = 0; i < roomCount;i++ )
                rooms.Add(new Room(avgRoomCapacity + rand.Next(avgRoomCapacity / 2) - avgRoomCapacity/4, i));

            for (int i = 0; i < timeSlotCount;i++ )
                timeSlots.Add(new TimeSlot(i));

            for (var i = 0; i < sessionCount; i++)
            {
                var sessionSpeakers = new List<Speaker>();
                bool reuse = rand.Next(sessionCount) < speakers.Count/3;
                sessionSpeakers.Add(FindSpeaker(speakers, reuse));

                var session = new Session(sessionSpeakers, "Session " + i);
                sessions.Add(session);
            }
            foreach(var session in sessions)
            {
                if (rand.Next(100) > 85)
                {
                    // Add another speaker!
                    var speaker = FindSpeaker(speakers, true);
                    if (!session.Speakers.Contains(speaker))
                        session.Speakers.Add(speaker);

                    if (rand.Next(100) > 85)
                    {
                        // Add yet another speaker!
                        var additionalSpeaker = FindSpeaker(speakers,true);
                        if (!session.Speakers.Contains(additionalSpeaker))
                            session.Speakers.Add(additionalSpeaker);
                    }
                }
            }

            for (var i = 0; i < attendeeCount;i++ )
            {
                int count = timeSlotCount;
                for(int j=0;j<timeSlotCount;j++)
                {
                    if (rand.Next(100) > 85)
                        count--;
                }
                var attendeeSessions = new List<Session>();

                for(int j=0;j<count;j++)
                {
                    var session = sessions[rand.Next(sessions.Count)];
                    if(attendeeSessions.Contains(session))
                    {
                        j--;
                        continue;
                    }
                    attendeeSessions.Add(session);
                }
                attendees.Add(new Attendee(attendeeSessions, i));
            }

            return new CodeCamp(attendees, sessions, speakers, rooms, timeSlots);
        }

        private static Speaker FindSpeaker(List<Speaker> speakers, bool resuse = false)
        {
            Speaker speaker = null;
            var rand = new Random((int)DateTime.Now.Ticks);
            if (speakers.Count > 0 && resuse)
            {
                // re-use existing speaker
                speaker = speakers[rand.Next(speakers.Count)];
            }
            else
            {
                speaker = new Speaker("Speaker " + (speakers.Count + 1));
                speakers.Add(speaker);
            }
            return speaker;
        }
    }
}