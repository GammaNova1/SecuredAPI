﻿using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache(); //Arka planda IMemoryCache instance oluşturur
                                                //serviceCollection.AddSingleton<IHttpContextAccessor, IHttpContextAccessor>();
          
        }
    }
}
