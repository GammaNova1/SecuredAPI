using Core.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        IDataResult<List<Role>> GetClaims(User user);
        IDataResult<List<UserDetailDto>> GetUserDetail(Expression<Func<User, bool>> filter);
    }
}
