namespace Zen.Application.Common.Dtos;

public interface IConcurrencyAwareDto
{
    string RowVersion { get; set; }
}
