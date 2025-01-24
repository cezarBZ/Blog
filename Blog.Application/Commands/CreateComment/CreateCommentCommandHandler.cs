using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using MediatR;

namespace Blog.Application.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Response<Unit>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserContextService _userContextService;

        public CreateCommentCommandHandler(IUserContextService userContextService, ICommentRepository commentRepository)
        {
            _userContextService = userContextService;
            _commentRepository = commentRepository;
        }

        public async Task<Response<Unit>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
                return Response<Unit>.NotFound("Usuário não está logado.");

            var comment = new Comment(request.Content, request.postId, userId.Value);

            await _commentRepository.AddAsync(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário criado com sucesso.");
        }
    }
}
