using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;
        public AuthorizationBehaviour(
            ICurrentUserService currentUserService,
            IApplicationDbContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttribute = request.GetType().GetCustomAttribute<AuthorizeAttribute>();

            if (authorizeAttribute != null)
            {
                if (_currentUserService.UserId == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var authorized = false;
                foreach (var role in authorizeAttribute.Roles)
                {
                    bool parsed = int.TryParse(_currentUserService.UserId, out int userId);

                    if (parsed)
                    {
                        var isInRole = await _context.Users.AllAsync(u => u.Id == userId && u.Role == role);
                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }

                }

                // Must be a member of at least one role in roles
                if (!authorized)
                {
                    throw new ForbiddenAccessException();
                }
            }

            // User is authorized / authorization not required
            return await next();
        }
    }
}
