using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions  //Araya girmesini istediğimiz servislerin koleksiyonu
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection serviceCollection, ICoreModule[] modules) //this ile yazılan kısım neyi genişletmek istediğimiz , parametre değil. virgülden sonrası parametre
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollection);
            }


            return ServiceTool.Create(serviceCollection);
        }


    }

}
