
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = EntityLayer.Concrete.User;


namespace DataAccessLayer.Concrete
{
    public class UserDal : GenericRepository<User>, IUserDal
    {
        private readonly IConfiguration _configuration;
        DbContextOptions<DbContext> options;
        public UserDal(DbContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public List<Role> GetClaims(User user)
        {
            if (user == null || user.Id == 0)
                throw new ArgumentNullException(nameof(user), "Kullanıcı bilgisi geçerli değil.");
            using (var context = new DbContext(options, _configuration))
            {
                var result = from role in context.Roles
                             join userrole in context.UserRoles
                             on role.Id equals userrole.RoleId
                             where userrole.UserId == user.Id
                             select new Role { Id = role.Id, Name = role.Name }; //join atıldı
                return result.ToList();

            }
        }

    }
}
