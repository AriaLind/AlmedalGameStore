using AlmedalGameStoreShared.Dtos.Payments;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.Repositories.MongoDb;

public class PaymentRepository : IRepository<PaymentDto, Guid>
{
    private readonly IMongoCollection<Payment> _collection;

    public PaymentRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Payment>("payments");
    }
    public async Task<PaymentDto> GetByIdAsync(Guid id)
    {
        var filter = Builders<Payment>.Filter.Eq(x => x.Id, id);

        var payment = await _collection.Find(filter).FirstOrDefaultAsync();

        PaymentDto paymentDto = new()
        {
            Type = payment.Type,
            Id = payment.Id
        };

        return paymentDto;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _collection.Find(_ => true).ToListAsync();

        List<PaymentDto> paymentDtos = new();

        foreach (var payment in payments)
        {
            PaymentDto paymentDto = new()
            {
                Type = payment.Type,
                Id = payment.Id
            };

            paymentDtos.Add(paymentDto);
        }

        return paymentDtos;
    }

    public async Task AddAsync(PaymentDto entity)
    {
        Payment payment = new()
        {
            Type = entity.Type,
            Id = entity.Id
        };

        await _collection.InsertOneAsync(payment);
    }

    public async Task UpdateAsync(PaymentDto entity)
    {
        var filter = Builders<Payment>.Filter.Eq(x => x.Id, entity.Id);

        var payment = await _collection.Find(filter).FirstOrDefaultAsync();

        payment.Type = entity.Type;
        payment.Id = entity.Id;

        await _collection.ReplaceOneAsync(filter, payment);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Payment>.Filter.Eq(x => x.Id, id);

        await _collection.DeleteOneAsync(filter);
    }
}