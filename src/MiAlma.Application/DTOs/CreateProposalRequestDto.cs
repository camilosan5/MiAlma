namespace MiAlma.Application.DTOs
{
    public class CreateProposalRequestDto
    {
        public Guid RfpId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
