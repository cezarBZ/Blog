using Blog.Application.Commands.PostCommands.CreatePost;
using Blog.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;

namespace Blog.API.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
           IConfiguration configuration)
        {

            services.AddMediatR(typeof(CreatePostCommand));
            services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<CreateCommentValidator>();

            return services;
        }
    }
}
