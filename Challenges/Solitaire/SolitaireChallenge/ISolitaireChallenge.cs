using System;
using System.Collections.Generic;
using System.Text;
using OmahaMTG.Challenge.ChallengeCommon;
namespace OmahaMTG.Challenge.SolitaireChallenge
{
    /// <summary>
    /// Interface to implement when solving the Solitaire Challenge.
    /// </summary>
    [CLSCompliant(false)]
    public interface ISolitaireChallenge : IChallenge
    {
        /// <summary>
        /// Should return an enumeration of moves that will solve the given Solitaire deal.
        /// </summary>
        /// <param name="solitaire"></param>
        /// <returns></returns>
        IEnumerable<Move> Solve(Solitaire solitaire);
    }
}