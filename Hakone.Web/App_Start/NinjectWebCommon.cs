using Hakone.Web.OAuth;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Hakone.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Hakone.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Hakone.Web.App_Start
{
    using System;
    using System.Web;
    using Hakone.Service;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IWorkContext>().To<WebWorkContext>().InRequestScope();
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<IShopService>().To<ShopService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IUserRoleService>().To<UserRoleService>();
            kernel.Bind<IShopCategoryService>().To<ShopCategoryService>();
            kernel.Bind<IProductCategoryService>().To<ProductCategoryService>();
            kernel.Bind<IUserFavProductService>().To<UserFavProductService>();
            kernel.Bind<IUserFavShopService>().To<UserFavShopService>();
            kernel.Bind<IShopCommentService>().To<ShopCommentService>();
            kernel.Bind<IOAuthService>().To<OAuthService>().InSingletonScope();
            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>().InSingletonScope();

        }        
    }
}
