using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using NSubstitute;
using NUnit.Framework;
using RestaurantManagement.BusinessLogic.Services;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _orderRepoMock = Substitute.For<IRepository<Order>>();
            _unitOfWorkMock.GetRepository<Order>().Returns(_orderRepoMock);

            _orderService = new OrderService(_dishServiceMock, _orderItemServiceMock,
                _mapper, _loggerManager, _unitOfWorkMock);
        }

        [Test]
        public async Task GetOrders_InvokesAppropriateRepositoryMethod()
        {
            //Arrange /Act
            await _orderService.GetOrders();

            //Assert
            await _orderRepoMock.Received().Get();
        }

        [Test]
        public async Task GetOrderById_MakesAppropriateCall()
        {
            //Arragne, Act
            await _orderService.GetOrderById(5);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>());
        }

        [Test]
        public async Task GetOrderWithItems_MakesAppropriateCall()
        {
            //Arragne, Act
            await _orderService.GetOrderWithItems(5);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<>());
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
