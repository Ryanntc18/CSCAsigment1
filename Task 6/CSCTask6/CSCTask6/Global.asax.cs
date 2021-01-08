using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Stripe;

namespace CSCTask6
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StripeConfiguration.ApiKey = "sk_test_51I5B7mLaXDcHpE2BKAPeNbNK9ZiHUVIaTI6D6YuXe1Mloa2F3GiIGXG0piKKs2vROyem94HywDr9ysJK0I8w0NRk00SZjaoGWh";
            
        }
    }
}
