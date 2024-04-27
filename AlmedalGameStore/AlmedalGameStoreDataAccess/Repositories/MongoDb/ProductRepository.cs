using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Dtos.Reviews;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.Repositories.MongoDb;

public class ProductRepository : IRepository<ProductDto, Guid>
{
    private readonly IMongoCollection<Product> _collection;

    public ProductRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Product>("products");
    }


    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, id);

        var product = await _collection.Find(filter).FirstOrDefaultAsync();

        ProductDto productDto = new()
        {
            ReleaseDate = product.ReleaseDate,
            CoverPicturePath = product.CoverPicturePath,
            Categories = product.Categories,
            AgeRequirement = product.AgeRequirement,
            Price = product.Price,
            IsPhysical = product.IsPhysical,
            Description = product.Description,
            Name = product.Name,
            Reviews = product.Reviews != null
                ? product.Reviews.Select(review => new ReviewDto
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList()
                : null,

            Stock = product.Stock,
            UnitsSold = product.UnitsSold
        };

        return productDto;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {

        var products = await _collection.Find(_ => true).ToListAsync();

        List<ProductDto> productDtos = new();

        foreach (var product in products)
        {
            ProductDto productDto = new()
            {
                ReleaseDate = product.ReleaseDate,
                CoverPicturePath = product.CoverPicturePath,
                Categories = product.Categories,
                AgeRequirement = product.AgeRequirement,
                Price = product.Price,
                IsPhysical = product.IsPhysical,
                Description = product.Description,
                Name = product.Name,
                Stock = product.Stock,
                Id = product.Id,
                Reviews = product.Reviews != null
                    ? product.Reviews.Select(review => new ReviewDto
                    {
                        Rating = review.Rating,
                        Id = review.Id,
                        Description = review.Description,
                        Title = review.Title
                    }).ToList()
                    : null
            };

            productDtos.Add(productDto);
        }

        return productDtos;
    }

    public async Task AddAsync(ProductDto entity)
    {
        Product product = new()
        {
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            AgeRequirement = entity.AgeRequirement,
            Categories = entity.Categories,
            CoverPicturePath = entity.CoverPicturePath,
            ReleaseDate = entity.ReleaseDate,
            Stock = entity.Stock,
            IsPhysical = entity.IsPhysical,
            Reviews = entity.Reviews != null
                ? entity.Reviews.Select(review => new Review
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList() : null
        };

        await _collection.InsertOneAsync(product);
    }

    public async Task UpdateAsync(ProductDto entity)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, entity.Id);

        var product = await _collection.Find(filter).FirstOrDefaultAsync();

        product = new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            IsPhysical = entity.IsPhysical,
            AgeRequirement = entity.AgeRequirement,
            Categories = entity.Categories,
            CoverPicturePath = entity.CoverPicturePath,
            ReleaseDate = entity.ReleaseDate,
            Stock = entity.Stock,
            Reviews = entity.Reviews != null
                ? entity.Reviews.Select(review => new Review
                {
                    Rating = review.Rating,
                    Description = review.Description,
                    Title = review.Title
                }).ToList() : null
        };

        await _collection.ReplaceOneAsync(filter, product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}