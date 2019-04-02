using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using CM.Services.interfaces;
using Domain;

namespace CM.Services
{
    public class DiResolver
    {
        private static IContainer Container { get; set; }

        public static IContainer GetContainer()
        {
            return Container;
        }

        public static void BindDependency(Assembly assembly)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EfCardRepository>().As<ICardRepository>();
            builder.RegisterType<CardHandler>().As<ICardHandler>();
            builder.RegisterControllers(assembly);
            Container = builder.Build();
        }

    }
}
