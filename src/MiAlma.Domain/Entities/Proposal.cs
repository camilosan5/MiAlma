using MiAlma.Domain.Enums;

namespace MiAlma.Domain.Entities
{
    public class Proposal
    {
        public Guid Id { get; set; }
        public Guid RfpId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ProposalStatus Status { get; set; } = ProposalStatus.Draft;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Rfp Rfp { get; set; } = null!;
        public User Owner { get; set; } = null!;
    }
}
