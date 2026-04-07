using AutoMapper;
using eShop.DTOs.Products;
using eShop.DTOs.Suppliers;
using eShop.Entities;

namespace eShop.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<PostProductDto,Product>().ForMember(d => d.ProductName, m => m.MapFrom(s => s.Name));
        CreateMap<Product,GetProductsDto>().ForMember(d => d.Name, m => m.MapFrom(s => s.ProductName));
        CreateMap<PostSupplierDto,Supplier>().ForMember(d => d.SupplierName, m => m.MapFrom(s => s.Name));
        
    }

}
