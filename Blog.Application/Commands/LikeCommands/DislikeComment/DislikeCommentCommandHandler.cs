using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.LikeCommands.DislikeComment
{
    internal class DislikeCommandHandler : IRequestHandler<DislikeCommentCommand, Response<Unit>>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICommentRepository _commentRepository;

        public DislikeCommandHandler(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor, IUserContextService userContextService, ICommentRepository commentRepository)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
            _commentRepository = commentRepository;
        }
        public async Task<Response<Unit>> Handle(DislikeCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
            {
                return new Response<Unit>(false, "Usuário não encontrado.");
            }

            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment == null)
            {
                return new Response<Unit>(false, "Comment não encontrado.");
            }

            var like = await _likeRepository.GetLikeByUserIdAndTargetIdAsync(userId.Value, request.CommentId, LikeTargetType.Comment);
            if (like == null)
            {
                return new Response<Unit>(false, "Você ainda não curtiu esse comment.");
            }

            comment.DecrementLikeCount();
            _likeRepository.Delete(like);
            _commentRepository.Update(comment);

            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comment descurtido com sucesso.");
        }
    }
}
