using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication17.Controllers;
using WebApplication17.DTOs;
using WebApplication17.Services;
using WebApplication17.Models;
using System;

namespace WebApplication17.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private IProductService _mockService;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = Substitute.For<IProductService>();
            _controller = new ProductsController(_mockService);
        }

       
        [Test]
        public async Task GetAll_ReturnsOk_WithProducts()
        {
            _mockService.GetAllAsync()
                .Returns(Task.FromResult<IEnumerable<Product>>(new List<Product>()));

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.GetById(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        
        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            _mockService.GetByIdAsync(1)
                .Returns(Task.FromResult<Product>(null));

            var result = await _controller.GetById(1);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

       
        [Test]
        public async Task GetById_ValidId_ReturnsOk()
        {
            _mockService.GetByIdAsync(1)
                .Returns(Task.FromResult(new Product { ProductId = 1, Name = "Laptop" }));

            var result = await _controller.GetById(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());


        }

        
        [Test]
        public async Task Create_InvalidModel_ReturnsBadRequest()
        {
            var dto = new ProductDTO();
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());


        }

        
        [Test]
        public async Task Create_ValidProduct_ReturnsOk()
        {
            var dto = new ProductDTO { Name = "Laptop" };

            var result = await _controller.Create(dto);

           
            await _mockService.Received(1).AddAsync(dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());


        }

        
        [Test]
        public async Task Create_Exception_ReturnsBadRequest()
        {
            var dto = new ProductDTO { Name = "Laptop" };

            _mockService.AddAsync(dto)
                .Returns(Task.FromException(new Exception("Error")));

            var result = await _controller.Create(dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());


        }

       
        [Test]
        public async Task Update_InvalidId_ReturnsBadRequest()
        {
            var dto = new ProductDTO();

            var result = await _controller.Update(0, dto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        
        [Test]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var dto = new ProductDTO { Name = "Laptop" };

            _mockService.UpdateAsync(1, dto)
                .Returns(Task.FromResult(false));

            var result = await _controller.Update(1, dto);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());


        }

        
        [Test]
        public async Task Update_Success_ReturnsOk()
        {
            var dto = new ProductDTO { Name = "Laptop" };

            _mockService.UpdateAsync(1, dto)
                .Returns(Task.FromResult(true));

            var result = await _controller.Update(1, dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());


        }

       
        [Test]
        public async Task Delete_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.Delete(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());


        }


        [Test]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            _mockService.DeleteAsync(1)
                .Returns(Task.FromResult(false));

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }


        [Test]
        public async Task Delete_Success_ReturnsOk()
        {
            _mockService.DeleteAsync(1)
                .Returns(Task.FromResult(true));

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());


        }
    }
}