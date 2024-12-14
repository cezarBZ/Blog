using Blog.Application.Commands.DeletePost;
using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.PostAggregate;
using MediatR;

namespace Blog.Application.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Response>
    {
        private readonly IPostRepository repository;

        public DeletePostCommandHandler(IPostRepository _repository)
        {
            repository = _repository;
        }

        public async Task<Response> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await repository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                var errorResponse = new Response { Success = false, Message = "Post not found" };
                return errorResponse;
            }

            repository.Delete(post);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var response = new Response { Success = true, Message = "Post deleted succesfuly" };

            return response;
        }
    }
}
