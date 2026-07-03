using MiAlma.Domain.Enums;

namespace MiAlma.Domain.Policies
{
    public static class ProposalStatusPolicy
    {
        private static readonly Dictionary<ProposalStatus, ProposalStatus[]> _allowedTransitions = new()
        {
            { ProposalStatus.Draft, new[] { ProposalStatus.InReview } },
            { ProposalStatus.InReview, new[] { ProposalStatus.Submitted } },
            { ProposalStatus.Submitted, new[] { ProposalStatus.Won, ProposalStatus.Lost } },
            { ProposalStatus.Won, new ProposalStatus[0] },
            { ProposalStatus.Lost, new ProposalStatus[0] }
        };

        public static bool CanTransition(ProposalStatus from, ProposalStatus to)
        {
            return _allowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
        }
    }
}
