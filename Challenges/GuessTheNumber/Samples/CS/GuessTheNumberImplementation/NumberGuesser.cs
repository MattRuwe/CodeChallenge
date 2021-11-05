using System;
using System.Collections.Generic;
using System.Linq;
using OmahaMTG.Challenge.GuessTheNumberChallenge;

namespace GuessTheNumberImplementation
{
    public class NumberGuesser : IGuessTheNumberChallenge
    {
        public string AuthorNotes
        {
            get { return string.Empty; }
        }

        public int GuessNumber(IEnumerable<int> sequence, Func<int, bool> checkAnswer)
        {
            int answer = 0;

            for (int i = 0; i <= 20; i++)
                if (checkAnswer(answer))
                    break;
                else
                    answer += 1;

            return answer;
        }

    }

}
