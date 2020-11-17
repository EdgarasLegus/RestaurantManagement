using AutoMapper;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
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
            var orderEntity = await Create(orderCreateEntity);
            var dishIdsList = await _orderItemRepo.GetDishesIdsByOrderId(orderEntity.Id);
            var orderedDishList = await _dishService.GetOrderedDishes(dishIdsList);

            if(OrderHasSomeItemsWithZeroStock(orderedDishList))
            {
                orderEntity = await PartiallyDecline(orderCreateEntity, orderedDishList);
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            else if (OrderHasAllItemsWithZeroStock(orderedDishList))
            {
                orderEntity = await Decline(orderCreateEntity, orderedDishList);
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            else
            {
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
        }

        public async Task DeleteCustomerOrder(int id)
        {
            var orderEntity = await _orderRepo.GetOrderById(id);
            await _orderRepo.DeleteOrder(orderEntity);
            _loggerManager.LogInfo($"DeleteCustomerOrder(): Order '{orderEntity.OrderName}' was deleted!");
        }

        public async Task<Order> GetExistingOrder(int id)
        {
            return await _orderRepo.GetOrderWithItems(id);
        }

        public async Task UpdateCustomerOrder(OrderUpdateModel orderUpdateEntity, int orderId)
        {
            if(orderUpdateEntity.IsPreparing == false)
            {
                await Update(orderUpdateEntity, orderId);
            }
            else
            {
                await Prepare(orderUpdateEntity, orderId);
            }
        }

        private bool OrderHasSomeItemsWithZeroStock(List<Dish> orderedDishList)
        {
            return orderedDishList.Any(x => x.QuantityInStock == 0) && !orderedDishList.All(x => x.QuantityInStock == 0);
        }

        private bool OrderHasAllItemsWithZeroStock(List<Dish> orderedDishList)
        {
            return orderedDishList.All(x => x.QuantityInStock == 0);
        }

        private async Task ChangeCreatedOrderStatus(Order orderEntity, List<Dish> orderedDishList)
        {
            var createdOrder = await _orderRepo.GetOrderByName(orderEntity.OrderName);
            await _orderRepo.UpdateOrder(createdOrder.Id, orderEntity);
            //var orderItemIdsList = GetSelectedOrderItemsIds(createdOrder.Id);
            await _orderItemService.UpdateCreatedOrderItemsStatuses(createdOrder.Id, orderedDishList);
        }

        private async Task<Order> Create(OrderCreateModel orderCreateEntity)
        {
            var orderEntity = _mapper.Map<Order>(orderCreateEntity);
            await _orderRepo.AddOrder(orderEntity);
            _loggerManager.LogInfo($"CreateCustomerOrder(): New order '{orderEntity.OrderName}' is successfully created! Status: 10");
            return orderEntity;
        }

        private async Task<Order> PartiallyDecline(OrderCreateModel orderCreateEntity, List<Dish> orderedDishList)
        {
            var orderPartiallyDeclinedEntity = _mapper.Map<OrderPartiallyDeclinedModel>(orderCreateEntity);
            var orderEntity = _mapper.Map<Order>(orderPartiallyDeclinedEntity);
            await ChangeCreatedOrderStatus(orderEntity, orderedDishList);
            _loggerManager.LogWarn($"CreateCustomerOrder(): Order '{orderEntity.OrderName}' was partially declined! Status: 20");
            return orderEntity;
        }

        private async Task<Order> Decline(OrderCreateModel orderCreateEntity, List<Dish> orderedDishList)
        {
            var orderDeclinedEntity = _mapper.Map<OrderDeclinedModel>(orderCreateEntity);
            var orderEntity = _mapper.Map<Order>(orderDeclinedEntity);
            await ChangeCreatedOrderStatus(orderEntity, orderedDishList);
            _loggerManager.LogWarn($"CreateCustomerOrder(): Order '{orderEntity.OrderName}' was declined! Status: 30");
            return orderEntity;
        }

        private async Task<List<int>> GetSelectedOrderItemsIds(int id)
        {
            return await _orderItemRepo.GetOrderItemIdsByOrderId(id);
        }

        private bool CheckIfOrderUnique(string orderName)
        {
            return _orderRepo.GetOrderByName(orderName) != null;
        }

        private async Task Update(OrderUpdateModel orderUpdateEntity, int id)
        {
            var orderEntity = _mapper.Map<Order>(orderUpdateEntity);
            await _orderRepo.UpdateOrder(id, orderEntity);
            _loggerManager.LogInfo($"Update(): Order '{orderEntity.OrderName}' was updated!");

        }

        private async Task Prepare(OrderUpdateModel orderUpdateEntity, int orderId)
        {
            await Update(orderUpdateEntity, orderId);
            await _orderItemService.AdjustOrderedItemsStock(orderId);
            await ChangePreparingOrderStatus(orderId);
        }

        private async Task ChangePreparingOrderStatus(int orderId)
        {
            await _orderRepo.UpdateOrderStatusAndDate(orderId, (int)OrderStates.Preparing);
            _loggerManager.LogInfo($"ChangePreparingOrderStatus(): Order {orderId} is preparing!");
        }

    }
}
