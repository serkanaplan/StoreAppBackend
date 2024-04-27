
namespace Store.Entities.Exceptions;

public abstract class NotFoundException(string message) : Exception(message)
{
}
