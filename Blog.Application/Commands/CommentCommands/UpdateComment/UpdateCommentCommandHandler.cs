using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Commands.CommentCommands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Response<Unit>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserContextService _userContextService;
        public UpdateCommentCommandHandler(IUserContextService userContextService, ICommentRepository commentRepository)
        {
            _userContextService = userContextService;
            _commentRepository = commentRepository;
        }
        public async Task<Response<Unit>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            User user = await _userContextService.GetUserAsync();

            var comment = await _commentRepository.GetByIdAsync(request.CommentId);

            if (comment == null)
                return Response<Unit>.NotFound("Comentário não encontrado");

            if (comment.UserId != user.Id)
                return Response<Unit>.Failure("Você não pode editar um comentário de outro usuário");


            comment.Edit(request.Content);

            _commentRepository.Update(comment);

            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Comentário editado com sucesso");
        }
    }
}
