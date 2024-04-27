using AlmedalGameStoreShared.Dtos.Orders;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.Repositories.MongoDb;

public class OrderRepository : IRepository<OrderDto, Guid>
{
    private readonly IMongoCollection<Order> _collection;

    public OrderRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Order>("orders");
    }

    public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

        var order = await _collection.Find(filter).FirstOrDefaultAsync();

        OrderDto orderDto = new()
        {
            Id = order.Id,
            Email = order.Email,
            PaymentId = order.PaymentId,
            ZipCode = order.ZipCode,
            Phonenumber = order.Phonenumber,
            Address = order.Address,
            City = order.City,
            Delivery = order.Delivery,
            CartId = order.CartId,
            PurchaseDate = order.PurchaseDate
        };

        return orderDto;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _collection.Find(_ => true).ToListAsync();

        List<OrderDto> orderDtos = new();

        foreach (var order in orders)
        {
            OrderDto orderDto = new()
            {
                Id = order.Id,
                Email = order.Email,
                PaymentId = order.PaymentId,
                ZipCode = order.ZipCode,
                Phonenumber = order.Phonenumber,
                Address = order.Address,
                City = order.City,
                Delivery = order.Delivery,
                CartId = order.CartId,
                PurchaseDate = new DateOnly()
            };

            orderDto.PurchaseDate = order.PurchaseDate;

            orderDtos.Add(orderDto);
        }

        return orderDtos;
    }

    public async Task AddAsync(OrderDto entity)
    {
        Order order = new()
        {
            Id = entity.Id,
            PaymentId = entity.PaymentId,
            CartId = entity.CartId,
            PurchaseDate = entity.PurchaseDate,
            Email = entity.Email,
            ZipCode = entity.ZipCode,
            Phonenumber = entity.Phonenumber,
            Address = entity.Address,
            City = entity.City,
            Delivery = entity.Delivery
        };

        await _collection.InsertOneAsync(order);
    }

    public async Task UpdateAsync(OrderDto entity)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, entity.Id);

        var order = await _collection.Find(filter).FirstOrDefaultAsync();

        order.Email = entity.Email;
        order.PaymentId = entity.PaymentId;
        order.ZipCode = entity.ZipCode;
        order.Phonenumber = entity.Phonenumber;
        order.Address = entity.Address;
        order.City = entity.City;
        order.Delivery = entity.Delivery;
        order.CartId = entity.CartId;

        await _collection.ReplaceOneAsync(filter, order);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

        await _collection.DeleteOneAsync(filter);
    }
}