using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using EntityLayer.Concrete;
using EntityLayer.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(string email);
        Task<IDataResult<User>> RegisterAsync(UserForRegisterDto userForRegisterDto, string password);

    }
}
