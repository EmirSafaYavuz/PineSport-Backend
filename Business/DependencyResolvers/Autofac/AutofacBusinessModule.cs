using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Business.Fakes.Handlers.OperationClaims;
using Business.Fakes.Handlers.User;
using Business.Fakes.Handlers.UserClaims;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Module = Autofac.Module;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            /*
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<PostService>().As<IPostService>();
            builder.RegisterType<EfPostDal>().As<IPostDal>();
            builder.RegisterType<PostBusinessRules>().AsSelf();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            */
            builder.RegisterType<LogRepository>().As<ILogRepository>();
            builder.RegisterType<TranslateRepository>().As<ITranslateRepository>();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>();
            
            
            builder.RegisterType<OperationClaimRepository>().As<IOperationClaimRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<UserClaimRepository>().As<IUserClaimRepository>();
            
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

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "Development")
            {
                // Development ortamında Fake sınıfları kullan
            }
            builder.RegisterType<InternalOperationClaimService>()
                .As<IInternalOperationClaimService>()
                .SingleInstance();

            builder.RegisterType<InternalUserService>()
                .As<IInternalUserService>()
                .SingleInstance();

            builder.RegisterType<InternalUserClaimService>()
                .As<IInternalUserClaimService>()
                .SingleInstance();
        }
    }
}
