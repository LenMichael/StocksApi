using FluentValidation;
using StocksApi.Dtos.Comments;

public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
{
    public UpdateCommentDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("The Title is required.")
            .MaximumLength(50).WithMessage("The Title must not exceed 50 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("The Content is required.")
            .MaximumLength(500).WithMessage("The Content must not exceed 500 characters.");
    }
}
