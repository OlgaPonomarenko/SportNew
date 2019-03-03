using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.Domain.Concrete;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1, Name = "P1" },
                new Product{ProductID = 2, Name = "P2" },
                new Product{ProductID = 3, Name = "P3" },
            });

            AdminController target = new AdminController(mock.Object);

            //act
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            //assign
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1, Name = "P1" },
                new Product{ProductID = 2, Name = "P2" },
                new Product{ProductID = 3, Name = "P3" },
            });

            AdminController target = new AdminController(mock.Object);

            //act
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            //assign
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1, Name = "P1" },
                new Product{ProductID = 2, Name = "P2" },
                new Product{ProductID = 3, Name = "P3" },
            });

            AdminController target = new AdminController(mock.Object);

            //act
            Product result = target.Edit(4).ViewData.Model as Product;

            //assign
            Assert.IsNull(result);            
        }

        [TestMethod]
        public void Can_Change_Valid_Changes()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            Product product = new Product { Name = "Test" };

            //Act
            ActionResult result = target.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(product));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            Product product = new Product { Name = "Test" };

            target.ModelState.AddModelError("error", "error");

            //Act
            ActionResult result = target.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Add_Product_To_ProductRepository()
        {
            //arrange
            ProductRepository repository = new ProductRepository();

            int repositorySize = repository.Products.Count();

            Product product = new Product() { ProductID = 0, Name = "Test" };

            //act
            repository.SaveProduct(product);

            //assert
            Assert.AreEqual(repositorySize+1, repository.Products.Count());            
        }

        [TestMethod]
        public void Can_Change_Product_At_The_ProductRepository()
        {
            //arrange
            ProductRepository repository = new ProductRepository();

            Product product = new Product
            {
                ProductID = 1,
                Name = "Football",
                Description = "Some description",
                Category = "Watersports",
                Price = 26
            };

            //act
            repository.SaveProduct(product);

            //assert
            Assert.AreEqual(repository.FindProductById(1).Price, 26);
        }

        [TestMethod]
        public void Can_Delete_Product()
        {
            //arrange
            Product prod = new Product { ProductID = 2, Name = "Test" };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1, Name = "P1" },
                prod,
                new Product{ProductID = 3, Name = "P3" }
            });

            AdminController target = new AdminController(mock.Object);

            //act
            target.Delete(prod.ProductID);

            //assert
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
