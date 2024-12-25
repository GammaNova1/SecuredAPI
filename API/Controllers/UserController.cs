using API.ApiResponse;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOS;
using EntityLayer.Models.UserModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
    
        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
            
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<ApiResponse<object>>> CreateUser([FromBody] UserForRegisterDto user)
        {
           
            User userToCreate = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                UserName = user.Email,
                Status = true
            };

            var result = await _userService.TInsert(userToCreate);
            if (result.IsSuccess)
            {
                
                return Ok(new ApiResponse<object>(default, "Kullanıcı başarıyla oluşturuldu.", HttpStatusCode.OK));
                
            }
            return BadRequest(new ApiResponse<object>("Kullanıcı oluşturulamadı", HttpStatusCode.OK));
        }
        [HttpGet("GetUserList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
 
        public async Task<ActionResult<ApiResponse<object>>> GetUserList()
        {
            var userList = _userService.TGetList();

            return Ok(new ApiResponse<object>(userList.Data, userList.Message, HttpStatusCode.OK));
        }

        [HttpGet("GetUserByMail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ApiResponse<object>>> GetUserByMail(string mail)
        {
            var user = _userService.TGet(x => x.Email == mail);

            return Ok(new ApiResponse<object>(user.Data, user.Message, HttpStatusCode.OK));
        }

        [HttpPut("UpdateUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ApiResponse<object>>> UpdateUser([FromBody] UpdateUserModel model)
        {
            var existingUser = _userService.TGet(x => x.Email == model.Email);
            existingUser.Data.Email = model.Email;
            existingUser.Data.Name = !string.IsNullOrWhiteSpace(model.Name) ? model.Name : existingUser.Data.Name;
            existingUser.Data.Surname = !string.IsNullOrWhiteSpace(model.Surname) ? model.Surname : existingUser.Data.Surname;
            await Task.Run(() => _userService.TUpdate(existingUser.Data));

            return Ok(new ApiResponse<object>(existingUser.Data.Id, "Güncelleme işlemi başarıı"));


        }
    }
}
