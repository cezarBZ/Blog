using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.LikeCommands.DislikePost
{
    public class DislikePostCommandHandler : IRequestHandler<DislikePostCommand, Response<Unit>>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContextService _userContextService;
        private readonly IPostRepository _postRepository;

        public DislikePostCommandHandler(ILikeRepository likeRepository, IUserContextService userContextService, IPostRepository postRepository)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
            _postRepository = postRepository;
        }
        public async Task<Response<Unit>> Handle(DislikePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();

            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                return new Response<Unit>(false, "Post não encontrado.");
            }

            var like = await _likeRepository.GetLikeByUserIdAndTargetIdAsync(userId.Value, request.PostId, LikeTargetType.Post);
            if (like == null)
            {
                return new Response<Unit>(false, "Você ainda não curtiu esse post.");
            }

            post.DecrementLikeCount();
            _likeRepository.Delete(like);
            _postRepository.Update(post);

            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Post descurtido com sucesso.");
        }
    }
}
