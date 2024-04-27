using AlmedalGameStoreShared.Dtos.Reviews;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.Repositories.MongoDb;

public class ReviewRepository : IRepository<ReviewDto, Guid>
{
    private readonly IMongoCollection<Review> _collection;

    public ReviewRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Review>("reviews");
    }
    public async Task<ReviewDto> GetByIdAsync(Guid id)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.Id, id);

        var review = await _collection.Find(filter).FirstOrDefaultAsync();

        ReviewDto reviewDto = new()
        {
            Rating = review.Rating,
            Title = review.Title,
            Description = review.Description,
        };
        return reviewDto;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync()
    {
        var reviews = await _collection.Find(_ => true).ToListAsync();

        List<ReviewDto> reviewDtos = new();

        foreach (var review in reviews)
        {
            ReviewDto reviewDto = new()
            {
                Rating = review.Rating,
                Title = review.Title,
                Description = review.Description,
                Id = review.Id
            };

            reviewDtos.Add(reviewDto);
        }
        return reviewDtos;
    }

    public async Task AddAsync(ReviewDto entity)
    {
        Review reviewDto = new()
        {
            Rating = entity.Rating,
            Title = entity.Title,
            Description = entity.Description,
        };

        await _collection.InsertOneAsync(reviewDto);
    }

    public async Task UpdateAsync(ReviewDto entity)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.Id, entity.Id);

        var review = await _collection.Find(filter).FirstOrDefaultAsync();

        review.Rating = entity.Rating;
        review.Title = entity.Title;
        review.Description = entity.Description;
        review.Id = entity.Id;


        await _collection.ReplaceOneAsync(filter, review);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}