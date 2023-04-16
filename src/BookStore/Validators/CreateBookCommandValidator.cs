using FluentValidation;

namespace BookStore.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(c => c.ISBN)
                .NotEmpty()
                .Matches("^\\d{13}$");

            RuleFor(c => c.Title)
                .NotEmpty();
        }
    }
}
