using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIS.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> productRepo,IMapper mapper,
            IGenericRepository<ProductType> typeRepo,
            IGenericRepository<ProductBrand> brandRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)

        {
            var spec= new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var mapperProducts = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products);
            //var returnedObject = new Pagination<ProductToReturnDto>() { 
            //    PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Data= mapperProducts
            //};
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize,mapperProducts));

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int Id)
        {
           // var product = await _productRepo.GetByIdAsync(Id);
           var spec = new ProductWithBrandAndTypeSpecifications(Id);
            var product =await _productRepo.GetByIdWithSpecAsync(spec);
            if(product == null) return NotFound(new ApiResponse(404));
            var mappedProduct=_mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(mappedProduct);
        }
        //GetAllTypes
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types =await _typeRepo.GetAllAsync();
            return Ok(Types);
        }
        //GetBrands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}
