﻿using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Users.Commands.BaseUser;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;
using Microsoft.AspNetCore.Identity;

namespace Core.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand : IBaseUser, IRequest<Result<UserDto>>
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class CreateUserCommandHandler :
        CreateBaseCommandHandler<CreateUserValidator, CreateUserCommand, UserDto, User>
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateUserCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, IPasswordHasher<User> pPasswordHasher,
            ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
            _passwordHasher = pPasswordHasher;
        }

        protected override async Task<User> Before(CreateUserCommand request)
        {
            var user = _mapper.Map<User>(request);
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            user.Type = User.UserType.SuperAdmin;

            return user;
        }
    }
}
