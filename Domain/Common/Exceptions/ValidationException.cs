namespace ReservationService.Domain.Common.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException()
        : base("Произошла одна или несколько ошибок валидации.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Произошла одна или несколько ошибок валидации.")
    {
        Errors = errors;
    }

    public ValidationException(string property, string message)
        : base("Произошла одна или несколько ошибок валидации.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { property, new[] { message } }
        };
    }
}
