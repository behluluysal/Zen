using System.Text.Json.Serialization;
using Zen.Domain.Aggregates;

namespace Zen.Services.Sso.Domain.UserAggregate;

public class AppUser : ZenUser
{
    #region [ Base Properties ]

    [JsonIgnore]
    public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

    [JsonIgnore]
    public override string? SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
    
    [JsonIgnore]
    public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

    #endregion

    public AppUser(string username, string email)
    {
        Id = Ulid.NewUlid().ToString();
        UserName = username;
        Email = email;
        IsDeleted = false;
        Raise(new UserCreatedEvent(this));
    }

    public void Update(string username, string email)
    {
        UserName = username;
        Email = email;
        //Raise(new ZenUserUpdatedEvent(this));
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        //Raise(new ZenUserDeletedEvent(this));
    }

    [JsonConstructor]
    private AppUser() { }
}