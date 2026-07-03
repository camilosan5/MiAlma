using System;
using MiAlma.Domain.Enums;

namespace MiAlma.Application.DTOs
{
    public class ProposalDto
    {
        public Guid Id { get; set; }
        public Guid RfpId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ProposalStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
