using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Response<Unit>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateCommentCommandHandler(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<Unit>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                                                .FirstOrDefault(c => c.Type == "userId");
            int userId = int.Parse(userIdClaim.Value);

            var post = await _postRepository.GetByIdAsync(request.PostId);

            if (post == null)
                return Response<Unit>.NotFound("Post não encontrado");

            var comment = post.Comments.FirstOrDefault(comment => comment.Id == request.CommentId);

            if (comment == null)
                return Response<Unit>.NotFound("Comentário não encontrado");

            if (comment.UserId != userId)
                return Response<Unit>.Failure("VocÊ não pode editar um comentário de outro usuário");


            comment.Edit(request.Content);

            _postRepository.EditCommentAsync(comment);

            await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário editado com sucesso");
        }
    }
}
