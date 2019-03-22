using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Mvc;
using CM.Services;
using CM.Services.interfaces.Abstract;
using Domain.Concrete;

namespace CM.Services
{
    public class DiResolver
    {
        private static IContainer Container { get; set; }

        public static IContainer GetContainer()
        {
            return Container;
        }

        public static void BindDependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EfCardRepository>().As<ICardRepository>();
            builder.RegisterType<CardHandler>().As<ICardHandler>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            Container = builder.Build();
        }

    }
}
