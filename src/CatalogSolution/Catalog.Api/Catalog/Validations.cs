using FluentValidation;

namespace Catalog.Api.Catalog;

public class CreateCatalogItemRequestValidator : AbstractValidator<CreateCatalogItemRequest>
{
    public CreateCatalogItemRequestValidator()
    {
        RuleFor(m => m.Version).NotEmpty();
        RuleFor(m => m.AnnualCostPerSeat).GreaterThan(0).When(m => m.IsCommercial);
    }
}

public class CatalogItemResponseValidator : AbstractValidator<CatalogItemResponse>
{
    public CatalogItemResponseValidator()
    {
        RuleFor(m => m.Application).NotEmpty().MaximumLength(100);
        RuleFor(m => m.Vendor).NotEmpty().MaximumLength(100);
        RuleFor(m => m.Version).NotEmpty().MaximumLength(10);
    }
}