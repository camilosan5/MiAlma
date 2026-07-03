namespace MiAlma.Domain.Exceptions
{
    public class ProposalNotEditableException : Exception
    {
        public ProposalNotEditableException(string message)
            : base(message)
        {
        }
    }
}
