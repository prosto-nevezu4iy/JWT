using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(
            IApplicationDbContext context, 
            IEmailService emailService, 
            ITokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.Email.Substring(0, request.Email.IndexOf("@")),
                Email = request.Email,
                VerificationToken = _tokenService.GenerateRandomToken(),
                PasswordHash = BC.HashPassword(request.Password),
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync(cancellationToken);

            //await _emailService.SendVerificationEmailAsync(user);

            return Unit.Value;
        }
    }
}
