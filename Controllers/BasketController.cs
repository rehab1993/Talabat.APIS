using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIS.Controllers
{
   
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        //Get Or Recreate
        [HttpGet("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>>  GetCustomerBasket(string BasketId)
        {
            var basket = await _basketRepository.GetBasketAsync(BasketId);
            return basket is null? new CustomerBasket(BasketId) : basket;
        }

        //Update Or Create
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
          var CreatedOrUpdatedBasket=  await _basketRepository.UpdateBasketAsync(basket);
            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return CreatedOrUpdatedBasket;

        }

        //DeleteBasket
        [HttpDelete]    
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
         return await  _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
