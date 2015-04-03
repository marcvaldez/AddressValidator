using AddressValidation.Framework.Interfaces;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace AddressValidation
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

#if _USPS
            builder.Register(s => new Framework.USPS.UspsAddressService(new Framework.USPS.UspsHttpClient()))
                .As<IAddressService>()
                .InstancePerRequest();
#elif _UPS
            builder.Register(s => new Framework.UPS.UpsAddressService(new Framework.UPS.UpsXavPortTypeClient()))
                .As<IAddressService>()
                .InstancePerRequest();
#elif _FEDEX
#endif

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}