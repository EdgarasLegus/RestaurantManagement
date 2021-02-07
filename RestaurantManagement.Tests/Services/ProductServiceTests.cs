using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using RestaurantManagement.BusinessLogic.Services;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Tests.Services
{
    public class ProductServiceTests
    {
        private IProductService _productService;
        private ILogicHandler _logicHandlerMock;
        private IUnitOfWork _unitOfWorkMock;
        private IRepository<Product> _productRepoMock;
        private IOptions<ConfigurationSettings> _optionsMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _productRepoMock = Substitute.For<IRepository<Product>>();
            _optionsMock = Substitute.For<IOptions<ConfigurationSettings>>();
            _unitOfWorkMock.GetRepository<Product>().Returns(_productRepoMock);
            _logicHandlerMock = Substitute.For<ILogicHandler>();


            _productService = new ProductService(_logicHandlerMock, _optionsMock, _unitOfWorkMock);
        }

        [Test]
        public async Task GetProducts_InvokesAppropriateRepositoryMethod()
        {
            //Arrange /Act
            await _productService.GetProducts();

            //Assert
            await _productRepoMock.Received().Get();
        }

        [Test]
        public async Task GetProductById_MakesAppropriateCall()
        {
            //Arragne, Act
            await _productService.GetProductById(5);

            //Assert
            await _productRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Product, bool>>>(),
                Arg.Any<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>());
        }

        [Test]
        public async Task AddProduct_MakesAppropriateCallOfRepositoryMethods()
        {
            //Arrange
            var productTestEntity = new Product
            {
                Id = 1,
                ProductName = "testProduct",
                StockAmount = 4.500M,
                UnitOfMeasurementId = 1
            };

            //Act
            await _productService.AddProduct(productTestEntity);
            //Assert
            await _productRepoMock.Received(1).Add(productTestEntity);
            await _unitOfWorkMock.Received(1).Commit();

        }
    }
}
