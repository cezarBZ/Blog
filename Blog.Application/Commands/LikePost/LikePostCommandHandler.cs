using Blog.Application.Responses;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LikePostCommandHandler(IPostRepository postRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<Unit>> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.postId);
            if (post == null)
            {
                return new Response<Unit>(false, "Post não encontrado.");
            }
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                                                .FirstOrDefault(c => c.Type == "userId");
            int userId = int.Parse(userIdClaim.Value);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new Response<Unit>(false, "Usuário não encontrado.");
            }

            var existingLike = await _postRepository.GetLikeByUserIdAndPostIdAsync(userId, request.postId);
            if (existingLike != null)
            {
                return new Response<Unit>(false, "Você já curtiu este post.");
            }

            var like = new Like(request.postId, userId);
            await _postRepository.AddLikeAsync(like);
            post.AddLike(like);
            _postRepository.Update(post);

            await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Post curtido com sucesso.");
        }
    }
}
