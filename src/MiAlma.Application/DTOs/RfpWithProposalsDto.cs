using System;
using System.Collections.Generic;

namespace MiAlma.Application.DTOs
{
    public class RfpWithProposalsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Agency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProposalDto> Proposals { get; set; } = new();
    }
}
