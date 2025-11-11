using DA;
using DA.Models;
using DAL.Interfaces;

namespace DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Product> Products { get; private set; }
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Order> Orders { get; private set; }
        public IRepository<OrderItem> OrderItems { get; private set; }
        public IRepository<CartItem> CartItems { get; private set; }
        public IRepository<Address> Addresses { get; private set; }
        public IRepository<Payment> Payments { get; private set; }
        public IRepository<Review> Reviews { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new Repository<Product>(_context);
            Categories = new Repository<Category>(_context);
            Orders = new Repository<Order>(_context);
            OrderItems = new Repository<OrderItem>(_context);
            CartItems = new Repository<CartItem>(_context);
            Addresses = new Repository<Address>(_context);
            Payments = new Repository<Payment>(_context);
            Reviews = new Repository<Review>(_context);
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
