using FluentValidation.Results;
using System.Runtime.Serialization;

namespace BookStore.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException() : base("One or more validation failures have occurred.") { }

        public ValidationException(string? message) : base(message) { }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(string? message, Exception? innerException) : base(message, innerException) { }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
    }
}
