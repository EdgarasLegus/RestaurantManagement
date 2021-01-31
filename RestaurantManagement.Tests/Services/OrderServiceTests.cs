using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using NSubstitute;
using NUnit.Framework;
using RestaurantManagement.BusinessLogic.Services;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
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
        private IMapper _mapperMock;
        private ILoggerManager _loggerManagerMock;
        private IUnitOfWork _unitOfWorkMock;
        private IRepository<Order> _orderRepoMock;
        private IRepository<OrderItem> _orderItemRepoMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _orderRepoMock = Substitute.For<IRepository<Order>>();
            _unitOfWorkMock.GetRepository<Order>().Returns(_orderRepoMock);
            _orderItemServiceMock = Substitute.For<IOrderItemService>();
            _dishServiceMock = Substitute.For<IDishService>();
            _mapperMock = Substitute.For<IMapper>();
            _loggerManagerMock = Substitute.For<ILoggerManager>();
            _orderItemRepoMock = Substitute.For<IRepository<OrderItem>>();


            _orderService = new OrderService(_dishServiceMock, _orderItemServiceMock,
                _mapperMock, _loggerManagerMock, _unitOfWorkMock);
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
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>());
        }

        [Test]
        public async Task GetOrderWithItems_MakesAppropriateCall()
        {
            //Arrange, Act
            await _orderService.GetOrderWithItems(5);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>());
        }

        [Test]
        public async Task AddOrder_MakesAppropriateCallOfRepositoryMethods()
        {
            //Arrange
            var orderTestEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            //Act
            await _orderService.AddOrder(orderTestEntity);
            //Assert
            await _orderRepoMock.Received(1).Add(orderTestEntity);

        }

        [Test]
        public async Task AddOrder_MakesCommitCall()
        {
            //Arrange
            var orderTestEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            //Act
            await _orderService.AddOrder(orderTestEntity);
            //Assert
            await _unitOfWorkMock.Received(1).Commit();
        }

        [Test]
        public async Task Create_InnerMethodsWereCalled()
        {
            //Arrange
            var testCreateOrderEntity = new OrderCreateModel
            {
                OrderName = "testOrder",
                OrderItems = new List<OrderItemCreateModel>
                {
                    new OrderItemCreateModel{ OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItemCreateModel{OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItemCreateModel{OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            _mapperMock.Map<Order>(Arg.Any<OrderCreateModel>()).Returns(testOrderEntity);

            //Act
            await _orderService.Create(testCreateOrderEntity);

            //Assert
            _mapperMock.Received(1).Map<Order>(Arg.Any<OrderCreateModel>());
            _loggerManagerMock.Received(1).LogInfo(Arg.Any<string>());


        }

        [Test]
        public async Task CreateCustomerOrder_IsPartiallyDeclined()
        {
            //Arrange
            var testCreateOrderEntity = new OrderCreateModel
            {
                OrderName = "testOrder",
                OrderItems = new List<OrderItemCreateModel>
                {
                    new OrderItemCreateModel{ OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItemCreateModel{OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItemCreateModel{OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testDishIdsList = new List<int>() {1,2,3};
            var orderedDishList = new List<Dish>
            {
                new Dish{ Id = 1, DishName = "Paprikos sriuba", IsOnMenu = true, DishType = "Sriubos", QuantityInStock = 3},
                new Dish{ Id = 2, DishName = "Apelsinu sultys", IsOnMenu = true, DishType = "Sultys", QuantityInStock = 0},
                new Dish{ Id = 3, DishName = "Juodųjų serbentų suflė", IsOnMenu = true, DishType = "Desertai", QuantityInStock = 1}
            };

            var testOrderViewModel = new OrderViewModel
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.PartiallyDeclined,
                IsPreparing = false,
                IsReady = false,
                OrderItems = new List<OrderItemViewModel>
                {
                    new OrderItemViewModel{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3, OrderItemStatus = 10},
                    new OrderItemViewModel{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5, OrderItemStatus = 30},
                    new OrderItemViewModel{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1, OrderItemStatus = 10}
                }
            };

            var testResultOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.PartiallyDeclined,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3, OrderItemStatus = 10},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5, OrderItemStatus = 30},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1, OrderItemStatus = 10}
                }
            };

            _mapperMock.Map<Order>(Arg.Any<OrderCreateModel>()).Returns(testOrderEntity);
            _orderItemServiceMock.GetDishesIdsByOrderId(testOrderEntity.Id).Returns(testDishIdsList);
            _dishServiceMock.GetOrderedDishes(testDishIdsList).Returns(orderedDishList);
            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>()).Returns(testOrderEntity);
            _mapperMock.Map<OrderViewModel>(testResultOrderEntity).Returns(testOrderViewModel);

            //Act
            var result = await _orderService.CreateCustomerOrder(testCreateOrderEntity) ;

            //Assert
            Assert.That(result.OrderStatus, Is.EqualTo(OrderStates.PartiallyDeclined));
        }

        [Test]
        public async Task DeleteCustomerOrder_InnerMethodsAreCalled()
        {
            //Arrange
            var orderTestEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>()).Returns(orderTestEntity);

            //Act
            await _orderService.DeleteCustomerOrder(orderTestEntity.Id);

            //Assert
            _orderRepoMock.Received(1).Delete(Arg.Any<Order>());
            await _unitOfWorkMock.Received(1).Commit();
            _loggerManagerMock.Received(1).LogInfo($"DeleteCustomerOrder(): Order '{orderTestEntity.OrderName}' was deleted!");
        }

        [Test]
        public async Task UpdateCreatedOrder_OrderNameWasUpdated()
        {
            //Arrange
            var orderTestUpdatableEntity = new Order
            {
                OrderName = "myNameIsUpdated"
            };

            var existingTestOrder = new Order
            {
                Id = 1,
                OrderName = "myNameisTestOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>()).Returns(existingTestOrder);

            //Act
            await _orderService.UpdateCreatedOrder(existingTestOrder.Id, orderTestUpdatableEntity);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>());
            await _unitOfWorkMock.Received(1).Commit();

            Assert.AreEqual(existingTestOrder.OrderName, orderTestUpdatableEntity.OrderName);
        }

        [Test]
        public async Task UpdateOrderStatusAndDate_StatusWasUpdated()
        {
            //Arrange
            var existingTestOrder = new Order
            {
                Id = 1,
                OrderName = "myNameisTestOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            var newStatus = (int)OrderStates.Edited;

            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>()).Returns(existingTestOrder);

            //Act
            await _orderService.UpdateOrderStatusAndDate(existingTestOrder.Id, newStatus);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>());
            await _unitOfWorkMock.Received(1).Commit();

            Assert.AreEqual(existingTestOrder.OrderStatus, newStatus);
        }

        [Test]
        public async Task UpdateExistingOrder_AllPropertiesUpdated()
        {
            //Arrange
            var existingTestOrder = new Order
            {
                Id = 1,
                OrderName = "myNameisTestOrder",
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            var newStatus = (int)OrderStates.Edited;

            var orderTestNewEntity = new Order
            {
                Id = 1,
                OrderName = "myNameIsUpdated",
                OrderStatus = newStatus,
                IsPreparing = true,
                IsReady = false
            };

            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>()).Returns(existingTestOrder);

            //Act
            await _orderService.UpdateExistingOrder(existingTestOrder.Id, orderTestNewEntity, newStatus);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>());
            await _unitOfWorkMock.Received(1).Commit();

            Assert.That(existingTestOrder, Has.Property("Id").EqualTo(orderTestNewEntity.Id)
                & Has.Property("OrderName").EqualTo(orderTestNewEntity.OrderName)
                & Has.Property("OrderStatus").EqualTo(orderTestNewEntity.OrderStatus)
                & Has.Property("IsPreparing").EqualTo(orderTestNewEntity.IsPreparing)
                & Has.Property("IsReady").EqualTo(orderTestNewEntity.IsReady)
                & Has.Property("CreatedDate").EqualTo(orderTestNewEntity.CreatedDate)
                & Has.Property("ModifiedDate").EqualTo(orderTestNewEntity.ModifiedDate)
                & Has.Property("OrderItem").EqualTo(orderTestNewEntity.OrderItem));
        }

        [Test]
        public async Task UpdateOrderNameAndStatus_AllPropertiesUpdated()
        {
            //Arrange
            var existingTestOrder = new Order
            {
                OrderName = "myNameisTestOrder",
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false
            };

            var newStatus = (int)OrderStates.Edited;

            var orderTestNewEntity = new Order
            {
                OrderName = "myNameIsUpdated",
                OrderStatus = newStatus,
                ModifiedDate = DateTime.Now
            };

            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>()).Returns(existingTestOrder);

            //Act
            await _orderService.UpdateOrderNameAndStatus(existingTestOrder.Id, orderTestNewEntity, newStatus);

            //Assert
            await _orderRepoMock.Received(1).GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, Order>>>());
            await _unitOfWorkMock.Received(1).Commit();

            Assert.That(existingTestOrder, Has.Property("Id").EqualTo(orderTestNewEntity.Id)
                & Has.Property("OrderName").EqualTo(orderTestNewEntity.OrderName)
                & Has.Property("OrderStatus").EqualTo(orderTestNewEntity.OrderStatus)
                & Has.Property("ModifiedDate").EqualTo(orderTestNewEntity.ModifiedDate));
        }

        [Test]
        public async Task OrderHasSomeItemsWithZeroStock_IsTrue()
        {
            //Arrange
            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testDishIdsList = new List<int>() { 1, 2, 3 };
            var orderedDishList = new List<Dish>
            {
                new Dish{ Id = 1, DishName = "Paprikos sriuba", IsOnMenu = true, DishType = "Sriubos", QuantityInStock = 3},
                new Dish{ Id = 2, DishName = "Apelsinu sultys", IsOnMenu = true, DishType = "Sultys", QuantityInStock = 0},
                new Dish{ Id = 3, DishName = "Juodųjų serbentų suflė", IsOnMenu = true, DishType = "Desertai", QuantityInStock = 1}
            };

            _orderItemServiceMock.GetDishesIdsByOrderId(testOrderEntity.Id).Returns(testDishIdsList);
            _dishServiceMock.GetOrderedDishes(testDishIdsList).Returns(orderedDishList);

            //Act
            var result = await _orderService.OrderHasSomeItemsWithZeroStock(testOrderEntity);

            //Assert
            await _orderItemServiceMock.Received(1).GetDishesIdsByOrderId(testOrderEntity.Id);
            await _dishServiceMock.Received(1).GetOrderedDishes(testDishIdsList);

            Assert.AreEqual(result, true);
        }

        [Test]
        public async Task OrderHasSomeItemsWithZeroStock_IsFalse()
        {
            //Arrange
            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testDishIdsList = new List<int>() { 1, 2, 3 };
            var orderedDishList = new List<Dish>
            {
                new Dish{ Id = 1, DishName = "Paprikos sriuba", IsOnMenu = true, DishType = "Sriubos", QuantityInStock = 3},
                new Dish{ Id = 2, DishName = "Apelsinu sultys", IsOnMenu = true, DishType = "Sultys", QuantityInStock = 10},
                new Dish{ Id = 3, DishName = "Juodųjų serbentų suflė", IsOnMenu = true, DishType = "Desertai", QuantityInStock = 1}
            };

            _orderItemServiceMock.GetDishesIdsByOrderId(testOrderEntity.Id).Returns(testDishIdsList);
            _dishServiceMock.GetOrderedDishes(testDishIdsList).Returns(orderedDishList);

            //Act
            var result = await _orderService.OrderHasSomeItemsWithZeroStock(testOrderEntity);

            //Assert
            await _orderItemServiceMock.Received(1).GetDishesIdsByOrderId(testOrderEntity.Id);
            await _dishServiceMock.Received(1).GetOrderedDishes(testDishIdsList);

            Assert.AreEqual(result, false);
        }

        [Test]
        public async Task OrderHasAllItemsWithZeroStock_IsTrue()
        {
            //Arrange
            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testDishIdsList = new List<int>() { 1, 2, 3 };
            var orderedDishList = new List<Dish>
            {
                new Dish{ Id = 1, DishName = "Paprikos sriuba", IsOnMenu = true, DishType = "Sriubos", QuantityInStock = 0},
                new Dish{ Id = 2, DishName = "Apelsinu sultys", IsOnMenu = true, DishType = "Sultys", QuantityInStock = 0},
                new Dish{ Id = 3, DishName = "Juodųjų serbentų suflė", IsOnMenu = true, DishType = "Desertai", QuantityInStock = 0}
            };

            _orderItemServiceMock.GetDishesIdsByOrderId(testOrderEntity.Id).Returns(testDishIdsList);
            _dishServiceMock.GetOrderedDishes(testDishIdsList).Returns(orderedDishList);

            //Act
            var result = await _orderService.OrderHasAllItemsWithZeroStock(testOrderEntity);

            //Assert
            await _orderItemServiceMock.Received(1).GetDishesIdsByOrderId(testOrderEntity.Id);
            await _dishServiceMock.Received(1).GetOrderedDishes(testDishIdsList);

            Assert.AreEqual(result, true);
        }

        [Test]
        public async Task OrderHasAllItemsWithZeroStock_IsFalse()
        {
            //Arrange
            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testDishIdsList = new List<int>() { 1, 2, 3 };
            var orderedDishList = new List<Dish>
            {
                new Dish{ Id = 1, DishName = "Paprikos sriuba", IsOnMenu = true, DishType = "Sriubos", QuantityInStock = 0},
                new Dish{ Id = 2, DishName = "Apelsinu sultys", IsOnMenu = true, DishType = "Sultys", QuantityInStock = 0},
                new Dish{ Id = 3, DishName = "Juodųjų serbentų suflė", IsOnMenu = true, DishType = "Desertai", QuantityInStock = 1}
            };

            _orderItemServiceMock.GetDishesIdsByOrderId(testOrderEntity.Id).Returns(testDishIdsList);
            _dishServiceMock.GetOrderedDishes(testDishIdsList).Returns(orderedDishList);

            //Act
            var result = await _orderService.OrderHasAllItemsWithZeroStock(testOrderEntity);

            //Assert
            await _orderItemServiceMock.Received(1).GetDishesIdsByOrderId(testOrderEntity.Id);
            await _dishServiceMock.Received(1).GetOrderedDishes(testDishIdsList);

            Assert.AreEqual(result, false);
        }

        [Test]
        public async Task PartiallyDecline_SetsStatusToPartiallyDeclinedToOrderAndDeclinedToItem()
        {
            //Arrange
            var testOrderEntity = new Order
            {
                Id = 1,
                OrderName = "testOrder",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                OrderStatus = (int)OrderStates.Created,
                IsPreparing = false,
                IsReady = false,
                OrderItem = new List<OrderItem>
                {
                    new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
                }
            };

            var testOrderItemsList = new List<OrderItem>
            {
                new OrderItem{Id = 1001, OrderId = 1, DishId = 1, Quantity = 3},
                    new OrderItem{Id = 1002, OrderId = 1, DishId = 2, Quantity = 5},
                    new OrderItem{Id = 1003, OrderId = 1, DishId = 3, Quantity = 1}
            };

            var firstDish = new Dish { Id = 1, DishName = "First Dish", QuantityInStock = 3 };
            var secondDish = new Dish { Id = 2, DishName = "Second Dish", QuantityInStock = 0 };
            var thirdDish = new Dish { Id = 3, DishName = "Third Dish", QuantityInStock = 1 };

            _orderRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Order, bool>>>(),
                Arg.Any<Func<IQueryable<Order>, IIncludableQueryable<Order, object>>>()).Returns(testOrderEntity);

            _orderItemRepoMock.Get(Arg.Any<Expression<Func<OrderItem, bool>>>(),
              Arg.Any<Func<IQueryable<OrderItem>, IIncludableQueryable<OrderItem, object>>>()).Returns(testOrderItemsList);
            _orderItemServiceMock.GetOrderItemsByOrderId(testOrderEntity.Id).Returns(testOrderItemsList);

            _dishServiceMock.GetDishById(testOrderItemsList[0].Id).Returns(firstDish);
            _dishServiceMock.GetDishById(testOrderItemsList[1].Id).Returns(secondDish);
            _dishServiceMock.GetDishById(testOrderItemsList[2].Id).Returns(thirdDish);

            _orderItemRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<OrderItem, bool>>>(),
                Arg.Any<Func<IQueryable<OrderItem>, IIncludableQueryable<OrderItem, object>>>()).Returns(testOrderItemsList[0], testOrderItemsList[1], testOrderItemsList[2]);

            //Act 
            var result = await _orderService.PartiallyDecline(testOrderEntity);

            //Assert

            Assert.That(testOrderEntity.OrderStatus, Is.EqualTo((int)OrderStates.PartiallyDeclined));
            Assert.That(testOrderItemsList[1].OrderItemStatus, Is.EqualTo((int)OrderItemStates.Declined));


        }

    }

}
