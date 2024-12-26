using API.ApiResponse;
using BusinessLayer.Abstract;
using EntityLayer.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
           
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login(UserForLoginDto userForLoginDto)
        {

            var userToLogin = await _authService.Login(userForLoginDto);
            if (!userToLogin.IsSuccess)
            {
                return BadRequest(new ApiResponse<object>(userToLogin.Message, HttpStatusCode.BadRequest));
            }

            var result = _authService.CreateAccessToken(userForLoginDto.Email);
            if (result.IsSuccess)
            {

                return Ok(new ApiResponse<object>(result.Data, "Giriş başarılı.", HttpStatusCode.OK)); ;
            }

            return BadRequest(new ApiResponse<object>(result.Message, HttpStatusCode.BadRequest));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<object>>> Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {

                var registerResult = await _authService.RegisterAsync(userForRegisterDto, userForRegisterDto.PasswordHash);
                if (registerResult.IsSuccess)
                {

                    return Ok(new ApiResponse<object>(default, "Kullanıcı başarıyla oluşturuldu.", HttpStatusCode.OK));

                }


                return BadRequest(new ApiResponse<object>(registerResult.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

