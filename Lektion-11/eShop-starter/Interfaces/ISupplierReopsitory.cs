using eShop.DTOs.Suppliers;

namespace eShop.Interfaces;

public interface ISupplierReopsitory
{
    public Task<List<GetSuppliersDto>> ListAllSuppliers();
    public Task<GetSupplierDto> FindSupplier(int id);
    public Task<int> AddSupplier(PostSupplierDto supplier);


}
