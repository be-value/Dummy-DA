﻿using Data;

namespace Api
{
    public interface IProductData
    {
        Task<Product> AddProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> UpdateProduct(Product product);
    }

    public class ProductData : IProductData
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 10,
                Name = "Strawberries",
                Description = "16oz package of fresh organic strawberries",
                Quantity = 1
            },
            new Product
            {
                Id = 20,
                Name = "Sliced bread",
                Description = "Load of fresh sliced wheat bread",
                Quantity = 1
            },
            new Product
            {
                Id = 30,
                Name = "Apples",
                Description = "Bag of 7 fresh McIntosh apples",
                Quantity = 1
            }
        };

        private int GetRandomInt()
        {
            var random = new Random();
            return random.Next(100, 1000);
        }

        public Task<Product> AddProduct(Product product)
        {
            product.Id = GetRandomInt();
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product> UpdateProduct(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            _products[index] = product;
            return Task.FromResult(product);
        }

        public Task<bool> DeleteProduct(int id)
        {
            var index = _products.FindIndex(p => p.Id == id);
            _products.RemoveAt(index);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            return Task.FromResult(_products.AsEnumerable());
        }
    }
}
