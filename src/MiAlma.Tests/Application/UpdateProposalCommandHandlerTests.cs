using MiAlma.Application.Features.Proposals.Commands;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Enums;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;
using Moq;
using Xunit;

namespace MiAlma.Tests.Application
{
    public class UpdateProposalCommandHandlerTests
    {
        private static readonly Guid OwnerId = Guid.NewGuid();

        private static Proposal CreateProposal(ProposalStatus status) => new()
        {
            Id = Guid.NewGuid(),
            RfpId = Guid.NewGuid(),
            OwnerId = OwnerId,
            Title = "Original title",
            Content = "Original content",
            Status = status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        [Fact]
        public async Task Handle_ProposalInDraft_UpdatesTitleAndContent()
        {
            var proposal = CreateProposal(ProposalStatus.Draft);
            var repository = new Mock<IProposalRepository>();
            repository.Setup(r => r.GetByIdAsync(proposal.Id)).ReturnsAsync(proposal);

            var handler = new UpdateProposalCommandHandler(repository.Object);
            var command = new UpdateProposalCommand(proposal.Id, "New title", "New content", OwnerId);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal("New title", result.Title);
            Assert.Equal("New content", result.Content);
            repository.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProposalAlreadySubmitted_ThrowsProposalNotEditableException()
        {
            var proposal = CreateProposal(ProposalStatus.Submitted);
            var repository = new Mock<IProposalRepository>();
            repository.Setup(r => r.GetByIdAsync(proposal.Id)).ReturnsAsync(proposal);

            var handler = new UpdateProposalCommandHandler(repository.Object);
            var command = new UpdateProposalCommand(proposal.Id, "New title", "New content", OwnerId);

            await Assert.ThrowsAsync<ProposalNotEditableException>(() => handler.Handle(command, CancellationToken.None));
            repository.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Never);
        }
    }
}
