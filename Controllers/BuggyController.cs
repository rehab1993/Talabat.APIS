using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIS.Controllers
{
   
    public class BuggyController : APIBaseController
    {
        private readonly APIDbContext _dbcontext;

        public BuggyController(APIDbContext dbcontext) {
            _dbcontext = dbcontext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbcontext.products.Find(100);
           if(product == null)  return NotFound(new ApiResponse(404));
           return Ok(product);

        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError() {
            var product = _dbcontext.products.Find(100);
            var productToReturn=product.ToString();
            return Ok(productToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest() { 
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id) {
            return Ok();
        }
    }
}
