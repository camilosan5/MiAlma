using System;

namespace MiAlma.Application.DTOs
{
    public class RfpDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Agency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
