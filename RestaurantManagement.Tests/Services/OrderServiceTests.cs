using AutoMapper;
using NUnit.Framework;
using RestaurantManagement.BusinessLogic.Services;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
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

        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService(_dishServiceMock, _orderItemServiceMock,
                _mapper, _loggerManager, _unitOfWorkMock);

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
