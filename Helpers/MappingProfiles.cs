using AutoMapper;
using System.Net.Sockets;
using Talabat.APIS.DTOS;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIS.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<Address, AddressDto>().ReverseMap();
            
            
        }
    }
}
