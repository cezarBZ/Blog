using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Response<Unit>>
    {
        private readonly ICommentRepository _commentRepository;
        public DeleteCommentCommandHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<Response<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);

            if (comment == null)
                return Response<Unit>.NotFound("comment not found");

            _commentRepository.Delete(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário deletado com sucesso");
        }
    }
}
