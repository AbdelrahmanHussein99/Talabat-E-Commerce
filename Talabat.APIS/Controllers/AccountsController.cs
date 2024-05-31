using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.APIS.Extebsions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIS.Controllers
{

    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
                                  SignInManager<AppUser> signInManager,
                                  ITokenService tokenService,
                                  IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model) 
        {

            if (CheckemailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "email is already exists"));
            }
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var ReturnedUser = new UserDto()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _tokenService.CreateTokenAsync(user,_userManager),
            };
            return Ok(ReturnedUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user =await _userManager.FindByEmailAsync(model.Email);

            if (user is null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            });

        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
         var userEmail=   User.FindFirstValue(ClaimTypes.Email );

            var user =await _userManager.FindByEmailAsync(userEmail);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            });
        }

        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
           

            var user = await _userManager.FindUserWithAddressAsync(User);
            var mappedAddress =_mapper.Map<Address,AddressDto>(user.Address);

            return Ok(mappedAddress);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto model)
        {


            var user = await _userManager.FindUserWithAddressAsync(User);

            var mappedAddress = _mapper.Map<AddressDto, Address>(model);


            user.Address = mappedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(result);

        }


        [HttpPut("emailExists")]
        public async Task<ActionResult<bool>> CheckemailExists(string email)
        {
            //var user = await _userManager.FindByEmailAsync(email);
            //if(user is null )return false;

            //return true;

            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
