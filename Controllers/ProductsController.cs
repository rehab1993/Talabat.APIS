using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIS.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductsController(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()

        {
            var Products = await _productRepo.GetAllAsync();
            //OkObjectResult result = new OkObjectResult(Products);
            return Ok(Products);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int Id)
        {
            var product = await _productRepo.GetByIdAsync(Id);
            return Ok(product);
        }
    }
}
