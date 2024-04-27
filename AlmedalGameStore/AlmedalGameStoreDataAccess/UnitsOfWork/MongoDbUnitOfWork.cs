using AlmedalGameStoreDataAccess.Repositories.MongoDb;
using MongoDB.Driver;

namespace AlmedalGameStoreDataAccess.UnitsOfWork;

public class MongoDbUnitOfWork
{
    private readonly IMongoDatabase _database;
    private readonly IClientSessionHandle _session;

    private ProductRepository _productRepository;
    private OrderRepository _orderRepository;
    private CartRepository _cartRepository;
    private EventRepository _eventRepository;
    private PaymentRepository _paymentRepository;
    private ReviewRepository _reviewRepository;

    public MongoDbUnitOfWork()
    {
        var connectionString = Environment.GetEnvironmentVariable("MongoDbConnectionString");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("MongoDbTest");
        _session = client.StartSession();
    }

    public ProductRepository ProductRepository
    {
        get
        {
            if (_productRepository == null)
            {
                _productRepository = new ProductRepository(_database);
            }
            return _productRepository;
        }
    }

    public OrderRepository OrderRepository
    {
        get
        {
            if (_orderRepository == null)
            {
                _orderRepository = new OrderRepository(_database);
            }
            return _orderRepository;
        }
    }

    public CartRepository CartRepository
    {
        get
        {
            if (_cartRepository == null)
            {
                _cartRepository = new CartRepository(_database);
            }
            return _cartRepository;
        }
    }

    public EventRepository EventRepository
    {
        get
        {
            if (_eventRepository == null)
            {
                _eventRepository = new EventRepository(_database);
            }
            return _eventRepository;
        }
    }

    public PaymentRepository PaymentRepository
    {
        get
        {
            if (_paymentRepository == null)
            {
                _paymentRepository = new PaymentRepository(_database);
            }
            return _paymentRepository;
        }
    }

    public ReviewRepository ReviewRepository
    {
        get
        {
            if (_reviewRepository == null)
            {
                _reviewRepository = new ReviewRepository(_database);
            }
            return _reviewRepository;
        }
    }
}