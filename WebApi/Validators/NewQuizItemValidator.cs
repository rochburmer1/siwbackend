using FluentValidation;
using WebApi.Dto;

namespace WebApi.Validators
{
    public class NewQuizItemValidator : AbstractValidator<NewQuizItemDto>
    {
        public NewQuizItemValidator()
        {
            RuleFor(q => q.Question)
                .NotEmpty().WithMessage("Pytanie nie może być puste.")
                .Length(3, 200).WithMessage("Pytanie musi mieć od 3 do 200 znaków.");

            RuleFor(q => q.Options)
                .NotEmpty().WithMessage("Lista opcji nie może być pusta.")
                .Must(options => options.Count >= 2).WithMessage("Muszą być co najmniej 2 opcje.")
                .Must(options => options.Count <= 10).WithMessage("Maksymalna liczba opcji to 10.");

            RuleFor(q => q.CorrectOptionIndex)
                .GreaterThanOrEqualTo(0).WithMessage("Indeks poprawnej odpowiedzi musi być liczbą nieujemną.")
                .Must((dto, index) => index < dto.Options.Count)
                .WithMessage("Indeks poprawnej odpowiedzi musi znajdować się w zakresie dostępnych opcji.");
        }
    }
}