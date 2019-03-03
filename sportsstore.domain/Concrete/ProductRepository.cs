using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class ProductRepository : IProductRepository
    {
        //public ProductRepository(IProductsMock productsMock)
        //{
            //_productsMock = productsMock;
        //}

        private List<Product> productList = new List<Product>()
        {
            new Product {ProductID = 0, Name = "Football1", Description = "Some description", Category = "Watersports", Price = 56},
            new Product {ProductID = 1, Name = "Football", Description = "Some description", Category = "Watersports", Price = 25},
            new Product {ProductID = 2, Name = "Surf board", Description = "Some description", Category = "Watersports", Price = 179},
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
            new Product {ProductID = 18, Name = "Product15", Description = "Some description", Category = "Soccer", Price = 79}
        };
        //private IProductsMock _productsMock;

        public IEnumerable<Product> Products
        {
            //get { return _productsMock.ProductList; }
            get { return productList; }
        }

        public void SaveProduct(Product product)
        {
            if (productList.Count == 0)
                AddProduct(product);

            if(product.ProductID == 0)
                AddProduct(product);

            else
            {
                Product requiredProduct = productList.Find(p => p.ProductID == product.ProductID);
                if (requiredProduct != null)
                {
                    requiredProduct.Name = product.Name;
                    requiredProduct.Description = product.Description;
                    requiredProduct.Price = product.Price;
                    requiredProduct.Category = product.Category;
                }
                else
                    AddProduct(product);
            }
        }

        public void AddProduct(Product product)
        {
            productList.Add(product);
        }

        public Product FindProductById(int productId)
        {
            return productList.Find(p => p.ProductID == productId);
        }

        public Product DeleteProduct(int productID)
        {
            Product requiredProduct = productList.Find(p => p.ProductID == productID);
            if (requiredProduct != null)
            {
                productList.Remove(requiredProduct);
            }
            return requiredProduct;
        }
    }
}
