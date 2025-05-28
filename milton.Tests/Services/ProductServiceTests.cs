using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using milton.Data;
using milton.Models.CompetitorPrices;

namespace milton.Tests.Services
{
    public class ProductServiceTests
    {
        private static Mock<DbSet<Product>> CreateMockDbSet(List<Product> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<Product>())).Callback<Product>(data.Add);
            mockSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<Product>>())).Callback<IEnumerable<Product>>(prods => data.AddRange(prods));
            mockSet.Setup(d => d.Update(It.IsAny<Product>())).Callback<Product>(p =>
            {
                var idx = data.FindIndex(x => x.Id == p.Id);
                if (idx >= 0) data[idx] = p;
            });
            mockSet.Setup(d => d.Remove(It.IsAny<Product>())).Callback<Product>(p => data.Remove(p));
            mockSet.Setup(d => d.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(ids => new ValueTask<Product>(data.FirstOrDefault(p => p.Id == (int)ids[0])));
            return mockSet;
        }

        private static Mock<ApplicationDbContext> CreateMockContext(List<Product> products)
        {
            var mockSet = CreateMockDbSet(products);
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            return mockContext;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectProduct()
        {
            var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var result = await service.GetByIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public async Task GetBySkuAsync_ReturnsCorrectProduct()
        {
            var products = new List<Product> { new Product { Id = 1, SKU = "A" }, new Product { Id = 2, SKU = "B" } };
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var result = await service.GetBySkuAsync("B");

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public async Task AddAsync_AddsProduct()
        {
            var products = new List<Product>();
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var product = new Product { Id = 1 };
            var result = await service.AddAsync(product);

            Assert.Single(products);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task AddRangeAsync_AddsProducts()
        {
            var products = new List<Product>();
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var newProducts = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            await service.AddRangeAsync(newProducts);

            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProduct()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Old" } };
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var updated = new Product { Id = 1, Name = "New" };
            await service.UpdateAsync(updated);

            Assert.Equal("New", products[0].Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesProduct()
        {
            var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            await service.DeleteAsync(1);

            Assert.Single(products);
            Assert.DoesNotContain(products, p => p.Id == 1);
        }

        [Fact]
        public async Task GetActiveAsync_ReturnsActiveProductsOrderedByName()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "B", Active = true },
                new Product { Id = 2, Name = "A", Active = true },
                new Product { Id = 3, Name = "C", Active = false }
            };
            var mockContext = CreateMockContext(products);
            var service = new ProductService(mockContext.Object);

            var result = await service.GetActiveAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("A", result[0].Name);
            Assert.Equal("B", result[1].Name);
        }
    }
}