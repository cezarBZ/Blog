using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.LikePost
{
    public class LikePostCommandHandler : IRequestHandler<LikePostCommand, Response<Unit>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;

        public LikePostCommandHandler(IPostRepository postRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUserContextService userContextService)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
        }
        public async Task<Response<Unit>> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.postId);
            if (post == null)
            {
                return new Response<Unit>(false, "Post não encontrado.");
            }

            var userId = _userContextService.GetUserId();
            if (userId == null)
            {
                return new Response<Unit>(false, "Usuário não encontrado.");
            }

            var existingLike = await _postRepository.GetLikeByUserIdAndPostIdAsync(userId.Value, request.postId);
            if (existingLike != null)
            {
                return new Response<Unit>(false, "Você já curtiu este post.");
            }

            var like = new Like(request.postId, userId.Value);
            await _postRepository.AddLikeAsync(like);
            post.AddLike(like);
            _postRepository.Update(post);

            await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Post curtido com sucesso.");
        }
    }
}
