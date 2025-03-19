using Zen.Domain.Events;

namespace Zen.Services.Sso.Domain.UserAggregate;

public record UserCreatedEvent(AppUser User) : ZenDomainEvent;