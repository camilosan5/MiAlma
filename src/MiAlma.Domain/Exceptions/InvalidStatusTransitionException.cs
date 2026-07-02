using MiAlma.Domain.Enums;

namespace MiAlma.Domain.Exceptions
{
    public class InvalidStatusTransitionException : Exception
    {
        public InvalidStatusTransitionException(ProposalStatus from, ProposalStatus to)
            : base($"Cannot transition from '{from}' to '{to}'.")
        {
        }
    }
}
