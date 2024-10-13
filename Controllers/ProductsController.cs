using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIS.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()

        {
            //var Products = await _productRepo.GetAllAsync();
            //OkObjectResult result = new OkObjectResult(Products);
            var spec= new ProductWithBrandAndTypeSpecifications();
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var mapperProducts = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(Products);
            return Ok(mapperProducts);

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
    }
}
