using AlmedalGameStoreShared.Dtos.Carts;
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Dtos.Reviews;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.Repositories.MongoDb;

public class CartRepository : IRepository<CartDto, Guid>
{
    private readonly IMongoCollection<Cart> _collection;

    public CartRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Cart>("carts");
    }

    public async Task<CartDto> GetByIdAsync(Guid id)
    {
        var filter = Builders<Cart>.Filter.Eq(x => x.Id, id);

        var cart = await _collection.Find(filter).FirstOrDefaultAsync();

        CartDto cartDto = new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            ProductDtoList = cart.ProductList != null
                        ? cart.ProductList.Select(product => new ProductDto
                        {
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            AgeRequirement = product.AgeRequirement,
                            ReleaseDate = product.ReleaseDate,
                            CoverPicturePath = product.CoverPicturePath,
                            Stock = product.Stock,
                            Id = product.Id,
                            Reviews = product.Reviews != null
                    ? product.Reviews?.Select(review => new ReviewDto
                    {
                        Rating = review.Rating,
                        Id = review.Id,
                        Description = review.Description,
                        Title = review.Title
                    }).ToList() : null,
                            Categories = product.Categories
                        }).ToList() : null,
            CheckedOut = cart.CheckedOut,
            Date = cart.Date
        };

        return cartDto;
    }

    public async Task<IEnumerable<CartDto>> GetAllAsync()
    {
        var carts = await _collection.Find(_ => true).ToListAsync();

        List<CartDto> cartDtos = new();

        foreach (var cart in carts)
        {

            CartDto cartDto = new CartDto
            {
                UserId = cart.UserId,
                ProductDtoList = cart.ProductList.Select(product => new ProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    AgeRequirement = product.AgeRequirement,
                    ReleaseDate = product.ReleaseDate,
                    CoverPicturePath = product.CoverPicturePath,
                    Stock = product.Stock,
                    Id = product.Id,
                    Reviews = product.Reviews?.Select(review => new ReviewDto
                    {
                        Rating = review.Rating,
                        Id = review.Id,
                        Description = review.Description,
                        Title = review.Title
                    }).ToList(),
                    Categories = product.Categories
                }).ToList(),
                CheckedOut = cart.CheckedOut,
                Date = cart.Date,
                Id = cart.Id
            };

            cartDtos.Add(cartDto);
        }
        return cartDtos;
    }

    public async Task AddAsync(CartDto entity)
    {
        Cart cart = new Cart
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProductList = entity.ProductDtoList.Select(productDto => new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                AgeRequirement = productDto.AgeRequirement,
                ReleaseDate = productDto.ReleaseDate,
                CoverPicturePath = productDto.CoverPicturePath,
                Stock = productDto.Stock,
                Id = productDto.Id,
                Reviews = productDto.Reviews?.Select(reviewDto => new Review
                {
                    Rating = reviewDto.Rating,
                    Id = reviewDto.Id,
                    Description = reviewDto.Description,
                    Title = reviewDto.Title
                }).ToList(),
                Categories = productDto.Categories
            }).ToList(),
            CheckedOut = entity.CheckedOut,
            Date = entity.Date
        };

        await _collection.InsertOneAsync(cart);
    }

    public async Task UpdateAsync(CartDto entity)
    {
        var filter = Builders<Cart>.Filter.Eq(x => x.Id, entity.Id);

        var cart = await _collection.Find(filter).FirstOrDefaultAsync();

        cart = new Cart
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProductList = entity.ProductDtoList != null
                ? entity.ProductDtoList.Select(productDto => new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    AgeRequirement = productDto.AgeRequirement,
                    ReleaseDate = productDto.ReleaseDate,
                    CoverPicturePath = productDto.CoverPicturePath,
                    Stock = productDto.Stock,
                    Id = productDto.Id,
                    Reviews = productDto.Reviews != null
                    ? productDto.Reviews?.Select(reviewDto => new Review
                    {
                        Rating = reviewDto.Rating,
                        Id = reviewDto.Id,
                        Description = reviewDto.Description,
                        Title = reviewDto.Title
                    }).ToList() : null,
                    Categories = productDto.Categories
                }).ToList() : null,
            CheckedOut = entity.CheckedOut,
            Date = entity.Date
        };

        await _collection.ReplaceOneAsync(filter, cart);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Cart>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}