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
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IDishService _dishService;
        private readonly IOrderRepo _orderRepo;

        public OrderItemService(IOrderItemRepo orderItemRepo, IMapper mapper, ILoggerManager loggerManager, IDishService dishService, IOrderRepo orderRepo)
        {
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _dishService = dishService;
            _orderRepo = orderRepo;
        }

        public async Task UpdateCreatedOrderItemsStatuses(int id, List<Dish> dishList)
        {
            foreach (var dish in dishList)
            {
                if (dish.QuantityInStock == 0)
                {
                    await _orderItemRepo.UpdateCreatedOrderItemStatus(id, dish.Id, (int)OrderItemStates.Declined);
                }
            }
        }

        public async Task<OrderItemViewModel> CreateCustomerOrderItem(int orderId, OrderItemCreateModel orderItemCreateEntity)
        {
            orderItemCreateEntity = await SetOrderIdForItem(orderId, orderItemCreateEntity);
            var orderItemEntity = await CreateItem(orderItemCreateEntity);
            await UpdateParentOrderStatusAndDate(orderItemEntity, (int)OrderStates.Edited);
            var orderedDish = await _dishService.GetSingleDish(orderItemEntity.DishId);

            if (orderedDish.QuantityInStock == 0)
            {
                orderItemEntity = await DeclineItem(orderItemEntity);
                return _mapper.Map<OrderItemViewModel>(orderItemEntity);
            }
            else
            {
                return _mapper.Map<OrderItemViewModel>(orderItemEntity);
            }
        }

        public async Task<OrderItem> GetSelectedOrderItem(int id)
        {
            return await _orderItemRepo.GetOrderItemById(id);
        }

        public async Task DeleteCustomerOrderItem(int itemId)
        {
            var orderItemEntity = await _orderItemRepo.GetOrderItemById(itemId);
            await _orderItemRepo.DeleteOrderItem(orderItemEntity);
            await UpdateParentOrderStatusAndDate(orderItemEntity, (int)OrderStates.Edited);
            _loggerManager.LogInfo($"DeleteCustomerOrderItem(): Order item {orderItemEntity.Id} was deleted!");
        }

        public async Task PrepareOrderItems(int orderId)
        {
            var orderItemsList = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
            foreach (var item in orderItemsList)
            {
                await PrepareOrderItem(item);
            }
        }

        public async Task ChangePreparingOrderItemStatus(OrderItem orderItemEntity)
        {
            await _orderItemRepo.UpdateOrderItemWithPreparingStatus(orderItemEntity, (int)OrderItemStates.Preparing);
            _loggerManager.LogInfo($"ChangePreparingOrderItemStatus(): Order item {orderItemEntity.Id} is preparing!");
        }

        private async Task<OrderItem> CreateItem(OrderItemCreateModel orderItemCreateEntity)
        {
            var orderItemEntity = _mapper.Map<OrderItem>(orderItemCreateEntity);
            await _orderItemRepo.AddOrderItem(orderItemEntity);
            _loggerManager.LogInfo($"CreateItem(): New item '{orderItemEntity.Id}' for selected order is successfully created! Status: 10");
            return orderItemEntity;
        }

        private async Task<OrderItem> DeclineItem(OrderItem orderItemEntity)
        {
            //var orderItemDeclinedEntity = _mapper.Map<OrderItemDeclinedModel>(orderItemCreateEntity);
            //var orderItemEntity = _mapper.Map<OrderItem>(orderItemDeclinedEntity);
            await UpdateAddedOrderItemStatus(orderItemEntity.Id, (int)OrderItemStates.Declined);
            _loggerManager.LogWarn($"DeclineItem(): Item '{orderItemEntity.Id}' for selected order was declined! Status: 30");
            return orderItemEntity;
        }

        private async Task UpdateParentOrderStatusAndDate(OrderItem orderItem, int status)
        {
            await _orderRepo.UpdateOrderStatusAndDate(orderItem.OrderId, status);
        }

        private async Task<OrderItemCreateModel> SetOrderIdForItem(int orderId, OrderItemCreateModel orderItemCreateEntity)
        {
            orderItemCreateEntity.OrderId = orderId;
            return orderItemCreateEntity;
        }

        private async Task UpdateAddedOrderItemStatus(int id, int status)
        {
            await _orderItemRepo.UpdatedAddedOrderItemStatus(id, status);
        }

        private async Task<bool> OrderItemStatusIsCreatedAndAvalaibleInStock(OrderItem orderItemEntity)
        {
            var orderedDish = await _dishService.GetSingleDish(orderItemEntity.DishId);
            return orderItemEntity.OrderItemStatus == 10 && orderedDish.QuantityInStock != 0;
        }

        private async Task PerformItemPreparation(OrderItem orderItemEntity)
        {
            await _dishService.AdjustDishStock(orderItemEntity.DishId, orderItemEntity.Quantity);
            _loggerManager.LogInfo($"AdjustItemsStockByStatus(): Dish ({orderItemEntity.DishId}) stock was adjusted.");
            await ChangePreparingOrderItemStatus(orderItemEntity);
        }

        private async Task DeclineItemPreparation(OrderItem orderItemEntity)
        {
            _loggerManager.LogWarn($"AdjustItemsStockByStatus(): Dish '{orderItemEntity.DishId}' stock hasn't been adjusted. " +
                    $"Order Item '{orderItemEntity.Id}' status is not created or dish '{orderItemEntity.DishId}' has 0 stock!" +
                    $"Order Item won't be prepared.");
            await DeclineItem(orderItemEntity);
        }

        private async Task PrepareOrderItem(OrderItem orderItemEntity)
        {
            if (await OrderItemStatusIsCreatedAndAvalaibleInStock(orderItemEntity))
            {
                await PerformItemPreparation(orderItemEntity);
            }
            else
            {
                await DeclineItemPreparation(orderItemEntity);
            }
        }

    }
}
