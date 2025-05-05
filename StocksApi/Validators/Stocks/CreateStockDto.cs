using FluentValidation;
using StocksApi.Dtos.Stocks;

public class CreateStockDtoValidator : AbstractValidator<CreateStockDto>
{
    public CreateStockDtoValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("The Symbol is required.")
            .MaximumLength(10).WithMessage("The Symbol must not exceed 10 characters.");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("The Company Name is required.")
            .MaximumLength(100).WithMessage("The Company Name must not exceed 100 characters.");

        RuleFor(x => x.Purchase)
            .GreaterThan(0).WithMessage("The Purchase value must be greater than 0.");

        RuleFor(x => x.LastDiv)
            .GreaterThanOrEqualTo(0).WithMessage("The Last Dividend must be 0 or greater.");

        RuleFor(x => x.Industry)
            .NotEmpty().WithMessage("The Industry is required.");

        RuleFor(x => x.MarketCap)
            .GreaterThan(0).WithMessage("The Market Cap must be greater than 0.");
    }
}
