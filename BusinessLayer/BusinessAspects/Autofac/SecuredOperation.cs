using Castle.DynamicProxy;
using Core.Utilities.Inteceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
namespace BusinessLayer.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception   //Authorization aspectler genelde business'da yazılır!
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor; //Birden fazla kişinin yaptığı isteklerde her kişi için ayrı bir context oluşturulur!

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();


            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception("Authorization Denied");
        }
    }
}
