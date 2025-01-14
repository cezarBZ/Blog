using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
    {
        private readonly IPostRepository _postRepository;

        public CreateCommentCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment(request.Content, request.IdPost, request.IdUser);

            await _postRepository.AddCommentAsync(comment);

            return Unit.Value;
        }
    }
}
