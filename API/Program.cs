
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Autofac;
using BusinessLayer.DependencyResolvers.Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using TokenOptions = Core.Utilities.Security.JWT.TokenOptions;
using Core.Utilities.IoC;
using Core.Extensions;
using Core.DependencyResolvers;
using DbContext = DataAccessLayer.DbContext;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var tokenOptionsSection = builder.Configuration.GetSection("TokenOptions");
            var tokenOptions = tokenOptionsSection.Get<TokenOptions>();
            // Add services to the container.
            try
            {

                builder.Services.AddControllers();

                builder.Services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
                
                builder.Services.AddEndpointsApiExplorer();

               
                builder.Services.AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SecureCoding", Version = "v1" });
                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
                });
                builder.Services.AddHttpContextAccessor();
                
                builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                //builder.Services.AddIdentityCore<User>()
                //   .AddEntityFrameworkStores<DataAccessLayer.DbContext>()
                //   .AddApiEndpoints();
                builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<DbContext>().AddDefaultTokenProviders();
               

                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                    options.AddPolicy("HelpDeskPolicy", policy => policy.RequireRole("Help_Desk"));
                });
                builder.Services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                        RoleClaimType = ClaimTypes.Role
                    };
                });




                builder.Services.AddDependencyResolvers(new ICoreModule[]
                {
                    new CoreModule()    //yeni module geldikçe virgülle new'leyip ekleyebiliriz!
                });
                ServiceTool.Create(builder.Services); //HttpContextAccessor þimdilik coreModule'de eklenemediði için geçici çözüm olarak bu bulunmuþtur!
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        builder => builder.AllowAnyOrigin()
                                          .AllowAnyMethod()
                                          .AllowAnyHeader());
                });


                builder.Host.UseServiceProviderFactory(Services => new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => { builder.RegisterModule(new AutofacBusinessModel()); });
                var app = builder.Build();

                if (app.Environment.IsProduction())
                    app.UseHsts();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
              
                
                app.UseHttpsRedirection();
                //app.MapIdentityApi<User>();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseCors("AllowAll");

                app.MapControllers();

                app.Run();
            }
            catch (Exception exception)
            {
                // logger.Error("Program.cs Exception: ", exception);
                throw new Exception("hata");
            }
            finally
            {
                throw new Exception();
            }

        }
    }
}