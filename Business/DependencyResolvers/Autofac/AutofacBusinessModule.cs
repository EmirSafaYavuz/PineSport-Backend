using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Module = Autofac.Module;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectDbContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();
            
            /*
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<PostService>().As<IPostService>();
            builder.RegisterType<EfPostDal>().As<IPostDal>();
            builder.RegisterType<PostBusinessRules>().AsSelf();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            */
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            })).AsSelf().SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope();

            // Aspect interceptor ayarları
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
