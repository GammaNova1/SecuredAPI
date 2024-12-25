using Autofac;
using Autofac.Extras.DynamicProxy;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Inteceptors;
using Core.Utilities.Security.JWT;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacBusinessModel : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<SignInManager<User>>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<UserManager<User>>().AsSelf().InstancePerLifetimeScope();



            builder.RegisterType<UserManager>().As<IUserService>().SingleInstance();
            builder.RegisterType<UserDal>().As<IUserDal>().SingleInstance();

           

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

           

            var assembly = System.Reflection.Assembly.GetExecutingAssembly(); //interceptor yapabilmek. Ascpetleri buradan çağıracağız!

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
