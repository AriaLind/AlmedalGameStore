﻿using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Carts.Delete;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Delete("/carts/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var carts = await mongoDbUnitOfWork.CartRepository.GetAllAsync();

        if (!carts.Any(p => p.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        await mongoDbUnitOfWork.CartRepository.DeleteAsync(req.Id);

        return TypedResults.Ok();
    }
}