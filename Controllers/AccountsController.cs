using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
    
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManger;
        private readonly ITokenService _tokenServices;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signinManger,
            ITokenService tokenServices)    
        {
            _userManager = userManager;
            _signinManger = signinManger;
            _tokenServices = tokenServices;
        }

        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var User = new AppUser()
            {
                DisplayName =model.DisplayName,
                Email =model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };

         var result= await  _userManager.CreateAsync(User,model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var returnedUser=new UserDto()
            {
                DisplayName=User.DisplayName,
                Email=User.Email,
                Token=await _tokenServices.CreateTokenAsync(User,_userManager)
            };
            return returnedUser;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User =await _userManager.FindByEmailAsync(model.Email);
            if (User == null) return Unauthorized(new ApiResponse(401));
           var result =await _signinManger.CheckPasswordSignInAsync(User,model.Password,false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenServices.CreateTokenAsync(User, _userManager)

            });


        }

        

    }
}
