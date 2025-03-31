using FluentValidation;
using WebApi.Dto;

namespace WebApi.Validators;

public class QuizItemValidator : AbstractValidator<QuizItemDto>
{
    public QuizItemValidator()
    {
        RuleFor(q => q.Question)
            .MaximumLength(200).WithMessage("Pytanie nie może być dłuższe niż 200 znaków.")
            .MinimumLength(3).WithMessage("Pytanie nie może być krótsze od 3 znaków!");

        RuleFor(q => q.Options)
            .NotNull().WithMessage("Opcje odpowiedzi są wymagane.")
            .Must(i => i.Count > 0).WithMessage("Musi być co najmniej jedna niepoprawna odpowiedź.");

        RuleFor(q => q.Options)
            .Must(i => i.Distinct().Count() == i.Count).WithMessage("Odpowiedzi nie mogą się powtarzać.");
    }
}