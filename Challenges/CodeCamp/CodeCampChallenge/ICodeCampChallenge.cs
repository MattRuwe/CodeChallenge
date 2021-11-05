using OmahaMTG.Challenge.ChallengeCommon;

namespace OmahaMTG.Challenge.Challenges
{
    public interface ICodeCampChallenge : IChallenge
    {
        ScheduledCodeCamp ScheduleSessions(CodeCamp codeCamp);
    }
}