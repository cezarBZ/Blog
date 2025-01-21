using Blog.Application.Services;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserContextService _userContextService;


        public CreateCommentCommandHandler(IPostRepository postRepository, IUserContextService userContextService)
        {
            _postRepository = postRepository;
            _userContextService = userContextService;
        }

        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
                return Unit.Value;

            var comment = new Comment(request.Content, request.postId, userId.Value);

            await _postRepository.AddCommentAsync(comment);
            await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
