using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
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
            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<BranchService>().As<IBranchService>();
            builder.RegisterType<ParentService>().As<IParentService>();
            builder.RegisterType<PaymentService>().As<IPaymentService>();
            builder.RegisterType<SchoolService>().As<ISchoolService>();
            builder.RegisterType<SessionService>().As<ISessionService>();
            builder.RegisterType<StudentService>().As<IStudentService>();
            builder.RegisterType<TrainerService>().As<ITrainerService>();
            builder.RegisterType<SessionService>().As<ISessionService>();
            
            builder.RegisterType<LogRepository>().As<ILogRepository>();
            builder.RegisterType<TranslateRepository>().As<ITranslateRepository>();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>();
            builder.RegisterType<OperationClaimRepository>().As<IOperationClaimRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<UserClaimRepository>().As<IUserClaimRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<RoleClaimRepository>().As<IRoleClaimRepository>();
            builder.RegisterType<SessionRepository>().As<ISessionRepository>();
            
            builder.RegisterType<BranchRepository>().As<IBranchRepository>();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>();
            builder.RegisterType<ParentRepository>().As<IParentRepository>();
            builder.RegisterType<SchoolRepository>().As<ISchoolRepository>();
            builder.RegisterType<TrainerRepository>().As<ITrainerRepository>();
            
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

            builder.RegisterType<InternalUserClaimService>()
                .As<IInternalUserClaimService>()
                .SingleInstance();
        }
    }
}
