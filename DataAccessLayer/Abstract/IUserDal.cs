using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using User = EntityLayer.Concrete.User; 

namespace DataAccessLayer.Abstract
{
    public interface IUserDal : IGenericDal<User>
    {
        List<Role> GetClaims(User user); // Eklenme sebebi burada bir join atılacak
        IQueryable<User> Query();
    }
}
