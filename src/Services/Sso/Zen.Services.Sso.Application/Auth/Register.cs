using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Zen.Services.Sso.Domain.UserAggregate;

namespace Zen.Services.Sso.Application.Auth;

internal sealed class RegisterUserCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<RegisterUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = new AppUser(command.UserName, command.Email);

        var identityResult = await userManager.CreateAsync(user, command.Password);

        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return Result.Error(string.Join(", ", errors));
        }

        return Result.Success(user.Id);
    }
}
