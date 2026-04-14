namespace Domain.Exceptions;
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string message) : base(message) { }
}
public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message) { }
}