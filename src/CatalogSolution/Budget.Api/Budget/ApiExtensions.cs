using Microsoft.AspNetCore.Http.HttpResults;

namespace Budget.Api.Budget;

public static class ApiExtensions
{
    public static IEndpointRouteBuilder MapBudgetRoutes(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/budget-allocations", HandleBudgetAllocations);
        routes.MapGet("/budget", GetBudget);

        return routes;
    }
    public static Results<Ok, BadRequest<string>> HandleBudgetAllocations(AllocateBudgetFor request)
    {
        var numberOfEmployees = 302;
        if (request.AnnualCostPerSeat * numberOfEmployees > 10000)
        {
            return TypedResults.BadRequest("We don't have enough budget");
        }
        else
        {
            // reduce the budget..
            return TypedResults.Ok();
        }
    }

    public static async Task<Ok<GetBudgetResponse>> GetBudget()
    {
        await Task.Delay(5000); // Simulate Don't Do This.
        var response = new GetBudgetResponse
        {
            RemainingBudget = 8233.23M,
            AnnualBudget = 12000.00M,
            AsOf = DateTimeOffset.UtcNow
        };

        return TypedResults.Ok(response);
    }
}


public record AllocateBudgetFor
{
    public string Vendor { get; set; } = string.Empty;
    public decimal AnnualCostPerSeat { get; set; }
}

public record GetBudgetResponse
{
    public DateTimeOffset AsOf { get; set; }
    public decimal RemainingBudget { get; set; }
    public decimal AnnualBudget { get; set; }
}