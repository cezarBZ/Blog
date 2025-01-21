using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Response<Unit>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserContextService _userContextService;
        public UpdateCommentCommandHandler(IPostRepository postRepository, IUserContextService userContextService)
        {
            _postRepository = postRepository;
            _userContextService = userContextService;
        }
        public async Task<Response<Unit>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            User user = await _userContextService.GetUserAsync();

            var post = await _postRepository.GetByIdAsync(request.PostId);

            if (post == null)
                return Response<Unit>.NotFound("Post não encontrado");

            var comment = post.Comments.FirstOrDefault(comment => comment.Id == request.CommentId);

            if (comment == null)
                return Response<Unit>.NotFound("Comentário não encontrado");

            if (comment.UserId != user.Id)
                return Response<Unit>.Failure("VocÊ não pode editar um comentário de outro usuário");


            comment.Edit(request.Content);

            _postRepository.EditCommentAsync(comment);

            await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário editado com sucesso");
        }
    }
}
