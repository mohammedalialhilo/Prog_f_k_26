namespace eShop.Interfaces;

public interface IUnitOfWork
{
    // IProductRepository ProductRepository { get; }

    Task<bool> Complete();
}
