using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.CommentCommands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Response<Unit>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        public DeleteCommentCommandHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        public async Task<Response<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);

            if (comment == null)
                return Response<Unit>.NotFound("comment not found");

            var post = await _postRepository.GetByIdAsync(comment.PostId);
            _commentRepository.Delete(comment);

            post.DecrementCommentCount();
            _postRepository.Update(post);

            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário deletado com sucesso");
        }
    }
}
