using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.CommentCommands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Response<Unit>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserContextService _userContextService;
        private readonly IPostRepository _postRepository;

        public CreateCommentCommandHandler(IUserContextService userContextService, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _userContextService = userContextService;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public async Task<Response<Unit>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
                return Response<Unit>.NotFound("Usuário não está logado.");

            var post = await _postRepository.GetByIdAsync(request.postId);
            if (post == null)
            {
                return new Response<Unit>(false, "Post não encontrado.");
            }

            var comment = new Comment(request.Content, request.postId, userId.Value);

            await _commentRepository.AddAsync(comment);

            post.IncrementCommentCount();
            _postRepository.Update(post);

            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário criado com sucesso.");
        }
    }
}
