using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.LikeAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.LikeCommands.LikePost
{
    public class LikePostCommandHandler : IRequestHandler<LikePostCommand, Response<Unit>>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContextService _userContextService;

        public LikePostCommandHandler(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor, IUserContextService userContextService)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
        }
        public async Task<Response<Unit>> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
            {
                return new Response<Unit>(false, "Usuário não encontrado.");
            }

            var existingLike = await _likeRepository.GetLikeByUserIdAndTargetIdAsync(userId.Value, request.postId, LikeTargetType.Post);
            if (existingLike != null)
            {
                return new Response<Unit>(false, "Você já curtiu este post.");
            }

            var like = new Like(request.postId, userId.Value, LikeTargetType.Post);
            await _likeRepository.AddAsync(like);

            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Post curtido com sucesso.");
        }
    }
}
