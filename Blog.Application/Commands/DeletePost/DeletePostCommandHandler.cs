using Blog.Application.Commands.DeletePost;
using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Response<Unit>>
    {
        private readonly IPostRepository repository;

        public DeletePostCommandHandler(IPostRepository _repository)
        {
            repository = _repository;
        }

        public async Task<Response<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await repository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                return Response<Unit>.NotFound();
            }

            repository.Delete(post);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var response = Response<Unit>.Success(Unit.Value, "Post deleted succesfuly");

            return response;
        }
    }
}
