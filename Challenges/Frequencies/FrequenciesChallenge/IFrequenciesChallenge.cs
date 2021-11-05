using System;
using System.Collections.Generic;
using System.Text;
using OmahaMTG.Challenge.ChallengeCommon;
namespace OmahaMTG.Challenge.FrequenciesChallenge
{
    /// <summary>
    /// Interface to implement when solving the Frequencies Challenge.
    /// </summary>
    public interface IFrequenciesChallenge : IChallenge
    {
        /// <summary>
        /// Given a list of stations return a list of frequencies these stations can broadcast on.
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        IEnumerable<FrequencyBand> AssignStations(IEnumerable<Station> stations);
    }
}