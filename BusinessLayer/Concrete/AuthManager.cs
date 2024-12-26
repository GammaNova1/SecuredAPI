using BusinessLayer.Abstract;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private UserManager<User> _userManager; //Identity userManager
        private IUserDal _userDal;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, UserManager<User> userManager, IUserDal userDal, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userManager = userManager;
            _userDal = userDal;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IDataResult<User>> RegisterAsync(UserForRegisterDto userForRegisterDto, string password)
        {
           
            var result = _userDal.GetAll(u => u.Email == userForRegisterDto.Email).Any();
            if (result)
            {
                return new ErrorDataResult<User>("Kullanıcı kayıtlı");
            }

          

            var user = new User
            {
                Email = userForRegisterDto.Email,
                Name = userForRegisterDto.Name,
                Surname = userForRegisterDto.Surname,
                UserName = userForRegisterDto.Email,
              
                Status = true
            };

            var res = await _userManager.CreateAsync(user, password);
            if (res.Succeeded == false)
            {
                string errorMessage = "";
                foreach (var i in res.Errors)
                {
                    errorMessage += i.Description + " ";
                }
                return new ErrorDataResult<User>(null, errorMessage);
            }
            var role = await _roleManager.FindByIdAsync("2");


            if (role != null)
            {

                var resp = await _userManager.AddToRoleAsync(user, role.Name);

            }
            return new SuccessDataResult<User>(user, "Kullanıcı Kayıt Edildi");
        }


        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {


            var result = _userDal.Get(u => u.Email == userForLoginDto.Email);
            if (result.Status == false)
            {
                return new ErrorDataResult<User>("Kullanıcı kayıtlı değil");
            }
            if (result == null)
            {
                string resultMessage = "Hata";

                return new ErrorDataResult<User>(null, resultMessage);
            }

            var res = await _userManager.FindByEmailAsync(userForLoginDto.Email);
            if (res != null)
            {
                var passwordValid = await _userManager.CheckPasswordAsync(res, userForLoginDto.Password);
                if (!passwordValid)
                {
                    return new ErrorDataResult<User>(null, "Şifre hatalı.");
                }
            }


            var result2 = await _signInManager.PasswordSignInAsync(userForLoginDto.Email, userForLoginDto.Password, false, false);


            //var us = _userService.TGet(a => a.Status);

            if (result2.Succeeded == false)
            {
                string errorMessage = "";

                return new ErrorDataResult<User>(null, errorMessage);
            }

            return new SuccessDataResult<User>(default, "Login Başarılı");
        }

        public IResult UserExists(string email)
        {

            if (_userService.TGet(x => x.Email == email).Data != null)
            {
                return new ErrorResult("Messages.UserAlreadyExists");
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(string email)
        {
            var user = _userService.TGet(x => x.Email == email);

            var claims = _userService.GetClaims(user.Data);
            var accessToken = _tokenHelper.CreateToken(user.Data, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, "Messages.AccessTokenCreated");
        }

    }
}
