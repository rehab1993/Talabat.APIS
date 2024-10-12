using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.Core.Entities;

namespace Talabat.APIS.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name));      
        }
    }
}
