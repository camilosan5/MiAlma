using MiAlma.Application.Features.Proposals.Commands;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Enums;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;
using Moq;
using Xunit;

namespace MiAlma.Tests.Application
{
    public class ChangeProposalStatusCommandHandlerTests
    {
        private static readonly Guid OwnerId = Guid.NewGuid();

        private static Proposal CreateProposal(ProposalStatus status) => new()
        {
            Id = Guid.NewGuid(),
            RfpId = Guid.NewGuid(),
            OwnerId = OwnerId,
            Title = "Test proposal",
            Content = "Test content",
            Status = status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        [Fact]
        public async Task Handle_ValidTransition_UpdatesStatusAndPersists()
        {
            var proposal = CreateProposal(ProposalStatus.Draft);
            var repository = new Mock<IProposalRepository>();
            repository.Setup(r => r.GetByIdAsync(proposal.Id)).ReturnsAsync(proposal);

            var handler = new ChangeProposalStatusCommandHandler(repository.Object);
            var command = new ChangeProposalStatusCommand(proposal.Id, ProposalStatus.InReview, OwnerId);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(ProposalStatus.InReview, result.Status);
            repository.Verify(r => r.UpdateAsync(It.Is<Proposal>(p => p.Status == ProposalStatus.InReview)), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidTransition_ThrowsInvalidStatusTransitionException()
        {
            var proposal = CreateProposal(ProposalStatus.Draft);
            var repository = new Mock<IProposalRepository>();
            repository.Setup(r => r.GetByIdAsync(proposal.Id)).ReturnsAsync(proposal);

            var handler = new ChangeProposalStatusCommandHandler(repository.Object);
            var command = new ChangeProposalStatusCommand(proposal.Id, ProposalStatus.Submitted, OwnerId);

            await Assert.ThrowsAsync<InvalidStatusTransitionException>(() => handler.Handle(command, CancellationToken.None));
            repository.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UserIsNotOwner_ThrowsForbiddenException()
        {
            var proposal = CreateProposal(ProposalStatus.Draft);
            var repository = new Mock<IProposalRepository>();
            repository.Setup(r => r.GetByIdAsync(proposal.Id)).ReturnsAsync(proposal);

            var handler = new ChangeProposalStatusCommandHandler(repository.Object);
            var command = new ChangeProposalStatusCommand(proposal.Id, ProposalStatus.InReview, Guid.NewGuid());

            await Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(command, CancellationToken.None));
            repository.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ProposalDoesNotExist_ThrowsNotFoundException()
        {
            var repository = new Mock<IProposalRepository>();
            repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Proposal?)null);

            var handler = new ChangeProposalStatusCommandHandler(repository.Object);
            var command = new ChangeProposalStatusCommand(Guid.NewGuid(), ProposalStatus.InReview, OwnerId);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
