﻿using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using MediatR;

namespace Blog.Application.Commands.UserCommands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        async Task<LoginUserViewModel> IRequestHandler<LoginUserCommand, LoginUserViewModel>.Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);
            var user = await _userRepository.GetUserByEmailAndPasswordAsync(request.Email, passwordHash);

            if (user == null)
                return null;

            var token = _authService.GenerateJwtToken(user.Email, user.Role, user.Id);

            user.RegisterLogin();
            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return new LoginUserViewModel(user.Email, token);
        }
    }
}
