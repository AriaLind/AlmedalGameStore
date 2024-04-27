using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Orders;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Orders.Add;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var order = new OrderDto()
        {
            Id = req.Id,
            PaymentId = req.PaymentId,
            CartId = req.CartId,
            PurchaseDate = req.PurchaseDate,
            Email = req.Email,
            Address = req.Address,
            City = req.City,
            Delivery = req.Delivery,
            Phonenumber = req.Phonenumber,
            ZipCode = req.ZipCode
        };

        if (order is null)
        {
            return TypedResults.BadRequest();
        }

        var allOrders = await mongoDbUnitOfWork.OrderRepository.GetAllAsync();

        if (allOrders.Any(o => o.Id.Equals(order.Id)))
        {
            return TypedResults.BadRequest();
        }

        await mongoDbUnitOfWork.OrderRepository.AddAsync(order);

        return TypedResults.Ok();
    }
}