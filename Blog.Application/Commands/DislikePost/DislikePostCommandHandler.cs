﻿using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.LikeAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Commands.DislikePost
{
    internal class DislikePostCommandHandler : IRequestHandler<DislikePostCommand, Response<Unit>>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContextService _userContextService;

        public DislikePostCommandHandler(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor, IUserContextService userContextService)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
        }
        public async Task<Response<Unit>> Handle(DislikePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
            {
                return new Response<Unit>(false, "Usuário não encontrado.");
            }

            var like = await _likeRepository.GetByIdAsync(request.Id);
            if (like == null)
            {
                return new Response<Unit>(false, "Você ainda não curtiu esse post.");
            }

            _likeRepository.Delete(like);

            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Post descurtido com sucesso.");
        }
    }
}
