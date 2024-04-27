using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Products;

namespace AlmedalGameStoreAuthApi.Endpoints.Products.GetAll;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<EmptyRequest, Results<Ok<Response>, BadRequest>>
{

    public override void Configure()
    {
        Get("/authProducts");
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreMongoDbApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("MongoDbApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync("/products");

        if (!response.IsSuccessStatusCode)
        {
            return TypedResults.BadRequest();
        }

        var products = await response.Content.ReadFromJsonAsync<ProductListDto>();

        return TypedResults.Ok(new Response
        {
            Products = products.Products
        });
    }
}