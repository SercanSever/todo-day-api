using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using TO_DO.SERVİCE.Concrete;
using TO_DO.SERVİCE.Contracts;
namespace TO_DO.SERVİCE.DependencyResolvers.Autofac
{
    public class TodoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TodoService>().As<ITodoService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
        }
    }
}