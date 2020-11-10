using AutoMapper;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDishService _dishService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public OrderService(IDishService dishService, IOrderItemService orderItemService, IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IMapper mapper, ILoggerManager loggerManager)
        {
            _dishService = dishService;
            _orderItemService = orderItemService;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<OrderViewModel> CreateCustomerOrder(OrderCreateModel orderCreateEntity)
        {
            var orderEntity = _mapper.Map<Order>(orderCreateEntity);
            await _orderRepo.AddOrder(orderEntity);
            _loggerManager.LogInfo($"CreateCustomerOrder(): New order {orderEntity.OrderName} is successfully created! Status: 10");

            var dishIdsList = await _orderItemRepo.GetOrderDishesByOrderId(orderEntity.Id);
            var orderedDishList = await _dishService.CalculateDishStock(dishIdsList);

            if(orderedDishList.Any(x => x.QuantityInStock == 0) && !orderedDishList.All(x => x.QuantityInStock == 0))
            {
                var orderPartiallyDeclinedEntity = _mapper.Map<OrderPartiallyDeclinedModel>(orderCreateEntity);
                orderEntity = _mapper.Map<Order>(orderPartiallyDeclinedEntity);

                var createdOrder = await _orderRepo.GetOrderByName(orderEntity.OrderName);
                await _orderRepo.UpdateOrder(createdOrder.Id, orderEntity);
                await _orderItemService.UpdateOrderItemsStatuses(createdOrder.Id, orderedDishList);
                _loggerManager.LogWarn($"CreateCustomerOrder(): Order {orderEntity.OrderName} was partially declined! Status: 20");

                return _mapper.Map<OrderViewModel>(orderEntity);

            }
            else if (orderedDishList.All(x => x.QuantityInStock == 0))
            {
                var orderDeclinedEntity = _mapper.Map<OrderDeclinedModel>(orderCreateEntity);
                orderEntity = _mapper.Map<Order>(orderDeclinedEntity);

                var createdOrder = await _orderRepo.GetOrderByName(orderEntity.OrderName);
                await _orderRepo.UpdateOrder(createdOrder.Id, orderEntity);
                await _orderItemService.UpdateOrderItemsStatuses(createdOrder.Id, orderedDishList);
                _loggerManager.LogWarn($"CreateCustomerOrder(): Order {orderEntity.OrderName} was declined! Status: 30");

                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            else
            {
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            
        }




        private bool CheckIfOrderUnique(string orderName)
        {
            var order = _orderRepo.GetOrderByName(orderName);
            if (order == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
