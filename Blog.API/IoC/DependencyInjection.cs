using Blog.Application.Commands.CreatePost;
using MediatR;
using System.Reflection;

namespace Blog.API.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
           IConfiguration configuration)
        {

            services.AddMediatR(typeof(CreatePostCommand));

            return services;
        }
    }
}
