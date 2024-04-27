using AlmedalGameStoreShared.Dtos.Event;
using AlmedalGameStoreShared.Dtos.Reviews;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.Repositories.MongoDb;

public class EventRepository : IRepository<EventDto, Guid>
{

    private readonly IMongoCollection<Event> _collection;

    public EventRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Event>("events");
    }

    public async Task<EventDto> GetByIdAsync(Guid id)
    {
        var filter = Builders<Event>.Filter.Eq(x => x.Id, id);

        var eventToGet = await _collection.Find(filter).FirstOrDefaultAsync();

        EventDto eventDto = new()
        {
            Id = eventToGet.Id,
            Name = eventToGet.Name,
            Description = eventToGet.Description,
            Price = eventToGet.Price,
            Reviews = eventToGet.Reviews != null
                ? eventToGet.Reviews.Select(review => new ReviewDto
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList()
                : null,
            Date = eventToGet.Date,
            Categories = eventToGet.Categories,
            CoverPicturePath = eventToGet.CoverPicturePath,
            TicketId = eventToGet.TicketId,
            Stock = eventToGet.Stock,
            UnitsSold = eventToGet.UnitsSold


        };

        return eventDto;
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync()
    {

        var allEvents = await _collection.Find(_ => true).ToListAsync();

        List<EventDto> eventDtos = new();

        foreach (var storedEvent in allEvents)
        {
            EventDto eventDto = new()
            {
                Id = storedEvent.Id,
                Categories = storedEvent.Categories,
                CoverPicturePath = storedEvent.CoverPicturePath,
                Date = storedEvent.Date,
                Description = storedEvent.Description,
                Name = storedEvent.Name,
                Stock = storedEvent.Stock,
                UnitsSold = storedEvent.UnitsSold,
                Price = storedEvent.Price,
                Reviews = storedEvent.Reviews != null
                    ? storedEvent.Reviews.Select(review => new ReviewDto()
                    {
                        Rating = review.Rating,
                        Id = review.Id,
                        Description = review.Description,
                        Title = review.Title
                    }).ToList()
                    : null,
                TicketId = storedEvent.TicketId

            };

            eventDtos.Add(eventDto);
        }

        return eventDtos;
    }

    public async Task AddAsync(EventDto entity)
    {
        Event eventObject = new()
        {
            Id = entity.Id,
            Categories = entity.Categories,
            CoverPicturePath = entity.CoverPicturePath,
            Date = entity.Date,
            Description = entity.Description,
            Name = entity.Name,
            Price = entity.Price,
            Stock = entity.Stock,
            UnitsSold = entity.UnitsSold,
            Reviews = entity.Reviews != null
                ? entity.Reviews.Select(review => new Review
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList()
                : null,
            TicketId = entity.TicketId

        };

        await _collection.InsertOneAsync(eventObject);
    }

    public async Task UpdateAsync(EventDto entity)
    {
        var filter = Builders<Event>.Filter.Eq(x => x.Id, entity.Id);

        var eventObject = await _collection.Find(filter).FirstOrDefaultAsync();

        eventObject = new()
        {
            Id = entity.Id,
            Categories = entity.Categories,
            CoverPicturePath = entity.CoverPicturePath,
            Date = entity.Date,
            Description = entity.Description,
            Name = entity.Name,
            Price = entity.Price,
            Stock = entity.Stock,
            UnitsSold = entity.UnitsSold,
            Reviews = entity.Reviews != null
                ? entity.Reviews.Select(review => new Review
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList()
                : null,
            TicketId = entity.TicketId

        };

        await _collection.ReplaceOneAsync(filter, eventObject);


    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Event>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}