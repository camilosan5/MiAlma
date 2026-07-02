using System;
using System.Collections.Generic;

namespace MiAlma.Domain.Entities
{
    public class Rfp
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Agency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    }
}
