using Blog.Application.Commands.LikeCommands.LikePost;
using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Blog.Application.Commands.LikeCommands.LikeComment
{
    internal class LikeCommentCommadHandler : IRequestHandler<LikeCommentCommand, Response<Unit>>
    {

        private readonly ILikeRepository _likeRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICommentRepository _commentRepository;

        public LikeCommentCommadHandler(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor, IUserContextService userContextService, ICommentRepository commentRepository)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
            _commentRepository = commentRepository;
        }
        public async Task<Response<Unit>> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
            {
                return new Response<Unit>(false, "Usuário não encontrado.");
            }

            var comment = await _commentRepository.GetByIdAsync(request.commentId);
            if (comment == null)
            {
                return new Response<Unit>(false, "Comment não encontrado.");
            }

            var existingLike = await _likeRepository.GetLikeByUserIdAndTargetIdAsync(userId.Value, request.commentId, LikeTargetType.Comment);
            if (existingLike != null)
            {
                return new Response<Unit>(false, "Você já curtiu este comentário.");
            }

            var like = new Like(request.commentId, userId.Value, LikeTargetType.Comment);

            comment.IncrementLikeCount();
            await _likeRepository.AddAsync(like);
            _commentRepository.Update(comment);

            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário curtido com sucesso.");
        }
    }
}
