using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Response<Unit>>
    {
        private readonly IPostRepository _postRepository;
        public DeleteCommentCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Response<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);

            if (post == null)
                return Response<Unit>.NotFound("Post not found");

            var comment = post.Comments.FirstOrDefault(c => c.Id == request.CommentId);

            if (comment == null)
                return Response<Unit>.NotFound("comment not found");

            _postRepository.DeleteComment(comment);
            await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário deletado com sucesso");
        }
    }
}
