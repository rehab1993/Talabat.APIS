using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
    
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManger;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signinManger,
            ITokenService tokenServices,IMapper mapper)    
        {
            _userManager = userManager;
            _signinManger = signinManger;
            _tokenServices = tokenServices;
            _mapper = mapper;
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
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)


            };
            return Ok(ReturedUser);

        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentAddress()
        {
          
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address,AddressDto>(user.Address);
            return Ok(MappedAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedaddress)
        {
            
            var user = await _userManager.FindUserWithAddressAsync(User);
          if(user == null) return Unauthorized(new ApiResponse(401));
            var address =  _mapper.Map<AddressDto, Address>(updatedaddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedaddress);
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email) {
            return await _userManager.FindByEmailAsync(email) is not null;


        }








    }
}
