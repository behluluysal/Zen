namespace Zen.Domain.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}

