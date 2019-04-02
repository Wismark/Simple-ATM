using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using CM.Services;

namespace Cash_Machine
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DiResolver.BindDependency(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new AutofacDependencyResolver(DiResolver.GetContainer()));
        }
    }
}
