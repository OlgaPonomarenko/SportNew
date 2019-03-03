using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Text;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1, Name = "P1" },
                new Product{ProductID = 2, Name = "P2" },
                new Product{ProductID = 3, Name = "P3" },
                new Product{ProductID = 4, Name = "P4" },
                new Product{ProductID = 5, Name = "P5" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //arrange
            HtmlHelper myHelper = null;

            //act
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            var expected = @"<a class=""btn btn-default"" href=""Page1"">1</a>"
             + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
             + @"<a class=""btn btn-default"" href=""Page3"">3</a>";
            var resultStr = result.ToString();
            //assert
            Assert.AreEqual(expected, resultStr);
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1" },
                new Product {ProductID = 2, Name = "P2" },
                new Product {ProductID = 3, Name = "P3" },
                new Product {ProductID = 4, Name = "P4" },
                new Product {ProductID = 5, Name = "P5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //assert
            PagingInfo pageInfo = result.pagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product {ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product {ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product {ProductID = 4, Name = "P4", Category = "Cat4" },
                new Product {ProductID = 5, Name = "P5", Category = "Cat5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            Product[] result = ((ProductsListViewModel)controller.List("Cat1", 1).Model).Products.ToArray();

            //assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P1" && result[0].Category == "Cat1");
            Assert.IsTrue(result[1].Name == "P3" && result[1].Category == "Cat1");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples" },
                new Product {ProductID = 2, Name = "P2", Category = "Apples" },
                new Product {ProductID = 3, Name = "P3", Category = "Plums" },
                new Product {ProductID = 4, Name = "P4", Category = "Oranges" }
            });

            NavController target = new NavController(mock.Object);

            //act
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            //assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Generate_Category_Spesific_Product_Count()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product {ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product {ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product {ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product {ProductID = 5, Name = "P5", Category = "Cat3" }
            });

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            //act
            int res1 = ((ProductsListViewModel)target.List("Cat1").Model).pagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).pagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List("Cat3").Model).pagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).pagingInfo.TotalItems;

            //assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
