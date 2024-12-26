using BusinessLayer.Abstract;
using BusinessLayer.BusinessAspects.Autofac;
using Core.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserManager(IUserDal userDal, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userDal = userDal;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IDataResult<List<Role>> GetClaims(User user)
        {
            return new SuccessDataResult<List<Role>>(_userDal.GetClaims(user));
        }

        public async Task<IResult> TDelete(User t)
        {
            _userDal.Delete(t);

            return new SuccessResult("Kullanıcı başarıyla silindi");
        }

        public IDataResult<User> TGet(Expression<Func<User, bool>> filter)
        {
            var user = _userDal.Get(filter);
            if (user == null)
            {
                return new ErrorDataResult<User>("Kullanıcı Bulunamadı");
            }
            return new SuccessDataResult<User>(user, "Kullanıcı Getirildi");
        }
        [SecuredOperation("Admin")]
        public IDataResult<List<User>> TGetList(Expression<Func<User, bool>> filter = null)
        {
            var userList = _userDal.GetAll(filter).ToList();

            if (userList.Count() <= 0)
            {
                return new ErrorDataResult<List<User>>("Kullanıcı listesi getirilemedi");
            }
            return new SuccessDataResult<List<User>>(userList, "Kullanıcı Listesi Getirildi");
        }

        public async Task<IResult> TInsert(User t)
        {
            var result = _userDal.GetAll(u => u.Email == t.Email).Any();
            if (result)
            {
                return new ErrorResult("Kullanıcı kayıtlı");
            }
           
            var resultCreated = await _userManager.CreateAsync(t, t.PasswordHash);

            if (resultCreated.Succeeded == false)
            {
                string errorMessage = "";
                foreach (var i in resultCreated.Errors)
                {
                    errorMessage += i.Description + " ";
                }
                return new ErrorResult(errorMessage);
            }
            var role = await _roleManager.FindByIdAsync("1");
            if (role != null)
            {
                var resp = await _userManager.AddToRoleAsync(t, role.Name);

            }
            return new SuccessResult("Kullanıcı Başarıyla Eklendi");
        }

        public async  Task<IResult> TUpdate(User t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            var existingUser = _userDal.Get(x => x.Id == t.Id);

            if (!string.IsNullOrEmpty(t.Name) || t.Name == "string")
            {
                t.Name = existingUser.Name;
            }

            if (!string.IsNullOrEmpty(t.Surname) || t.Surname == "string")
            {
                t.Surname = existingUser.Surname;
            }
            if (!string.IsNullOrEmpty(t.Email) && t.Email != existingUser.Email)
            {
                var emailValidator = new EmailAddressAttribute();
                if (!emailValidator.IsValid(t.Email))
                {
                    return new ErrorResult("Geçersiz bir e-posta adresi girdiniz.");
                }
            }


            if (!string.IsNullOrEmpty(t.PasswordHash))
            {

                if (t.PasswordHash.Length < 6)
                {
                    return new ErrorResult("Şifre en az 6 karakter olmalıdır.");
                }
                existingUser.PasswordHash = t.PasswordHash;
            }

            var result = await _userManager.UpdateAsync(t);

            if (result.Succeeded == false)
            {
                string errorMessage = "";
                foreach (var i in result.Errors)
                {
                    errorMessage += i.Description + " ";
                }
                return new ErrorResult("Kullanıcı Güncellenemedi");
            }

            return new SuccessResult("Kullanıcı Güncellendi");
        }
    }
}
