using DA.Models;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Address> Addresses { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Review> Reviews { get; }

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
