using core.Interfaces;

namespace infrastructure.Data;

public class UnitOfWork(EShopContext context) : IUnitOfWork
{
    // public IProductRepository ProductRepository => new ProductRepository(context);

    
    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
