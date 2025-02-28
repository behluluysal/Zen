namespace Zen.Application.Dtos;

public interface IConcurrencyAwareDto
{
    string RowVersion { get; set; }
}
