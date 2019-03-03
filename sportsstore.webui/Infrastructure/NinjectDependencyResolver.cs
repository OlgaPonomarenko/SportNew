using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver :IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            /*
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductID = 1, Name = "Football", Description = "Some description", Category = "Watersports", Price = 25},
                new Product {ProductID = 2, Name = "Surf board", Description = "Some description", Category = "Watersports", Price = 179},
                new Product {ProductID = 3, Name = "Running shoes", Description = "Some description", Category = "Soccer", Price = 467},            
                new Product {ProductID = 4, Name = "Product1", Description = "Some description", Category = "Soccer", Price = 45},
                new Product {ProductID = 5, Name = "Product2", Description = "Some description", Category = "Watersports", Price = 453},
                new Product {ProductID = 6, Name = "Product3", Description = "Some description", Category = "Soccer", Price = 57},
                new Product {ProductID = 7, Name = "Product4", Description = "Some description", Category = "Watersports", Price = 245},
                new Product {ProductID = 8, Name = "Product5", Description = "Some description", Category = "Soccer", Price = 4685},
                new Product {ProductID = 9, Name = "Product6", Description = "Some description", Category = "Watersports", Price = 78},
                new Product {ProductID = 10, Name = "Product7", Description = "Some description", Category = "Soccer", Price = 74},
                new Product {ProductID = 11, Name = "Product8", Description = "Some description", Category = "Watersports", Price = 245},
                new Product {ProductID = 12, Name = "Product9", Description = "Some description", Category = "Soccer", Price = 678},
                new Product {ProductID = 13, Name = "Product10", Description = "Some description", Category = "Watersports", Price = 44},
                new Product {ProductID = 14, Name = "Product11", Description = "Some description", Category = "Soccer", Price = 44675},
                new Product {ProductID = 15, Name = "Product12", Description = "Some description", Category = "Watersports", Price = 578},
                new Product {ProductID = 16, Name = "Product13", Description = "Some description", Category = "Soccer", Price = 58},
                new Product {ProductID = 17, Name = "Product14", Description = "Some description", Category = "Watersports", Price = 79},
                new Product {ProductID = 18, Name = "Product15", Description = "Some description", Category = "Soccer", Price = 29}
            });
            */

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);
            kernel.Bind<IProductRepository>().To<ProductRepository>().InSingletonScope();
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}