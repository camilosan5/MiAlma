using MiAlma.Domain.Enums;
using MiAlma.Domain.Policies;
using Xunit;

namespace MiAlma.Tests.Domain
{
    public class ProposalStatusPolicyTests
    {
        [Theory]
        [InlineData(ProposalStatus.Draft, ProposalStatus.InReview)]
        [InlineData(ProposalStatus.InReview, ProposalStatus.Submitted)]
        [InlineData(ProposalStatus.Submitted, ProposalStatus.Won)]
        [InlineData(ProposalStatus.Submitted, ProposalStatus.Lost)]
        public void CanTransition_AllowsValidForwardSteps(ProposalStatus from, ProposalStatus to)
        {
            Assert.True(ProposalStatusPolicy.CanTransition(from, to));
        }

        [Theory]
        [InlineData(ProposalStatus.Draft, ProposalStatus.Submitted)]
        [InlineData(ProposalStatus.Draft, ProposalStatus.Won)]
        [InlineData(ProposalStatus.InReview, ProposalStatus.Draft)]
        [InlineData(ProposalStatus.Submitted, ProposalStatus.Draft)]
        [InlineData(ProposalStatus.Won, ProposalStatus.Lost)]
        [InlineData(ProposalStatus.Lost, ProposalStatus.Won)]
        public void CanTransition_RejectsSkippedStepsBackwardsAndFinalStates(ProposalStatus from, ProposalStatus to)
        {
            Assert.False(ProposalStatusPolicy.CanTransition(from, to));
        }
    }
}
