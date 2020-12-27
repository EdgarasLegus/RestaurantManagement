using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using RestaurantManagement.BusinessLogic.Services;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.Tests.Services
{
    public class OrderServiceTests
    {
        private IOrderService _orderService;
        private IDishService _dishServiceMock;
        private IOrderItemService _orderItemServiceMock;
        private IMapper _mapper;
        private ILoggerManager _loggerManager;
        private IUnitOfWork _unitOfWorkMock;
        private IRepository<Order> _orderRepoMock;

        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService(_dishServiceMock, _orderItemServiceMock,
                _mapper, _loggerManager, _unitOfWorkMock);
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _orderRepoMock = _unitOfWorkMock.GetRepository<Order>();

        }

        [Test]
        public async Task Test_GetOrders_ReturnOkResult()
        {
            var order = new Order()
            {
                OrderName = "TestOrder",
                CreatedDate = new DateTime(2020, 12, 27),
                ModifiedDate = new DateTime(2020, 12, 28),
                OrderStatus = 10,
                IsPreparing = false,
                IsReady = false

            };
            var ordersList = new List<Order>() { order };

            _orderRepoMock.Get().Returns(Arg.Any<List<Order>>());

            var result = await _orderService.GetOrders();
            await _orderRepoMock.Received().Get();
            Assert.AreEqual(ordersList, result);

        }

        [Test]
        public async Task Test_CreateCustomerOrder_IfOrderIsPartiallyDeclined()
        {
            //Arrange
            var orderCreateModel = new OrderCreateModel
            {
                OrderName = "testOrder",
                OrderItems = new List<OrderItemCreateModel>
                {
                    new OrderItemCreateModel{DishId = 1, Quantity = 1},
                    new OrderItemCreateModel{DishId = 2, Quantity = 1},
                    new OrderItemCreateModel{DishId = 2, Quantity = 2}
                }
            };

            //Act

            //Assert
        }

    }
}
