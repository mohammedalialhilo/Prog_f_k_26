using eShop.DTOs.Suppliers;
using eShop.Interfaces;

namespace eShop.Mock;

public class MockSupplier : ISupplierRepository
{
    public Task<int> AddSupplier(PostSupplierDto supplier)
    {
        throw new NotImplementedException();
    }

    public Task<GetSupplierDto> FindSupplier(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<GetSuppliersDto>> ListAllSuppliers()
    {
    //    List<GetSuppliersDto> suppliers = [];
    //    suppliers.Add(new GetSuppliersDto{Id =1, Email="test@mail.com",Phone="010-1111",Name="Test supplier"});
    //    return Task.FromResult(suppliers);
        throw new NotImplementedException();
       
    }

    Task<bool> ISupplierRepository.AddSupplier(PostSupplierDto supplier)
    {
        throw new NotImplementedException();
    }

}
