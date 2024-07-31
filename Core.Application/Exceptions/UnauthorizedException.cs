﻿using Core.Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public int ErrorCode { get; } = StatusCodes.Status401Unauthorized;

        public UnauthorizedException(int errorCode) : base(Transforms.IdentityTransform.ForbiddenException())
        {
            ErrorCode = errorCode == StatusCodes.Status403Forbidden ? StatusCodes.Status403Forbidden : ErrorCode;
        }
    }

}
