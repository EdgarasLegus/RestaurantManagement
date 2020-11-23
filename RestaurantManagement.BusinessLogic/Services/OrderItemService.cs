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

        public async Task ReadyToServeOrderItems(int orderId)
        {
            var orderItemsList = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
            foreach (var item in orderItemsList)
            {
                await ReadyToServeOrderItem(item);
            }
        }

        private async Task ChangePreparingOrderItemStatus(OrderItem orderItemEntity)
        {
            await _orderItemRepo.UpdateOrderItemStatus(orderItemEntity, (int)OrderItemStates.Preparing);
            _loggerManager.LogInfo($"ChangePreparingOrderItemStatus(): Order item" +
                $" {orderItemEntity.Id} is preparing!");
        }

        private async Task<OrderItem> CreateItem(OrderItemCreateModel orderItemCreateEntity)
        {
            var orderItemEntity = _mapper.Map<OrderItem>(orderItemCreateEntity);
            await _orderItemRepo.AddOrderItem(orderItemEntity);
            _loggerManager.LogInfo($"CreateItem(): New item '{orderItemEntity.Id}' for" +
                $" selected order is successfully created! Status: 10");
            return orderItemEntity;
        }

        private async Task<OrderItem> DeclineItem(OrderItem orderItemEntity)
        {
            await UpdateAddedOrderItemStatus(orderItemEntity.Id, (int)OrderItemStates.Declined);
            _loggerManager.LogWarn($"DeclineItem(): Item '{orderItemEntity.Id}' for" +
                $" selected order was declined! Status: 30");
            return orderItemEntity;
        }

        private async Task<bool> ParentOrderHasAnyPreparingOrPreparedItems(int orderId)
        {
            var orderItemList = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
            return orderItemList.Any(x => x.OrderItemStatus == 50) || orderItemList.Any(x => x.OrderItemStatus == 60);
        }

        private async Task UpdateParentOrderStatusAndDate(OrderItem orderItem, int status)
        {
            if(await ParentOrderHasAnyPreparingOrPreparedItems(orderItem.OrderId))
            {
                await _orderRepo.UpdateOrderStatusAndDate(orderItem.OrderId, (int)OrderStates.Updated);
            }
            else
            {
                await _orderRepo.UpdateOrderStatusAndDate(orderItem.OrderId, status);
            }
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

        private async Task<bool> OrderItemIsPreparing(OrderItem orderItemEntity)
        {
            return orderItemEntity.OrderItemStatus == 50;
        }

        private async Task<bool> OrderItemIsAlreadyPreparingOrPrepared(OrderItem orderItemEntity)
        {
            return orderItemEntity.OrderItemStatus == 50 || orderItemEntity.OrderItemStatus == 60;
        }

        private async Task PerformItemPreparation(OrderItem orderItemEntity)
        {
            await _dishService.AdjustDishStock(orderItemEntity.DishId, orderItemEntity.Quantity);
            _loggerManager.LogInfo($"PerformItemPreparation(): Dish ({orderItemEntity.DishId}) stock" +
                $" was adjusted.");
            await ChangePreparingOrderItemStatus(orderItemEntity);
        }

        private async Task DeclineItemPreparation(OrderItem orderItemEntity)
        {
            _loggerManager.LogWarn($"DeclineItemPreparation(): Dish '{orderItemEntity.DishId}'" +
                $" stock hasn't been adjusted. " +
                    $"Order Item '{orderItemEntity.Id}' status is not created or dish" +
                    $" '{orderItemEntity.DishId}' has 0 stock!" +
                    $"Order Item won't be prepared.");
            await DeclineItem(orderItemEntity);
        }

        private async Task PrepareOrderItem(OrderItem orderItemEntity)
        {
            if (await OrderItemStatusIsCreatedAndAvalaibleInStock(orderItemEntity))
            {
                await PerformItemPreparation(orderItemEntity);
            }
            else if (await OrderItemIsAlreadyPreparingOrPrepared(orderItemEntity))
            {
                _loggerManager.LogWarn($"PrepareOrderItem(): Item '{orderItemEntity.Id}' cannot be" +
                    $" preparing! This item is already " +
                    $"in 'Preparing' or in 'ReadyToServe' status.");
            }
            else
            {
                await DeclineItemPreparation(orderItemEntity);
            }
        }

        private async Task ChangeReadyOrderItemStatus(OrderItem orderItemEntity)
        {
            await _orderItemRepo.UpdateOrderItemStatus(orderItemEntity, (int)OrderItemStates.ReadyToServe);
            _loggerManager.LogInfo($"ChangeReadyOrderItemStatus(): Order item {orderItemEntity.Id} is" +
                $" ready to serve!");
        }

        private async Task PerformReadyToServe(OrderItem orderItemEntity)
        {
            await ChangeReadyOrderItemStatus(orderItemEntity);
        }

        private async Task ReadyToServeOrderItem(OrderItem orderItemEntity)
        {
            if (await OrderItemIsPreparing(orderItemEntity))
            {
                await PerformReadyToServe(orderItemEntity);
            }
            else
            {
                _loggerManager.LogWarn($"ReadyToServerOrderItem(): Item '{orderItemEntity.Id}'" +
                    $" cannot be ready! This item is not " +
                    $"in 'Preparing' status or already in 'ReadyToServe' status.");
            }
        }
    }
}
