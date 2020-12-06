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

        public async Task UpdateCreatedOrderItemsStatuses(int orderId)
        {
            var orderItemsList = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
            foreach (var item in orderItemsList)
            {
                await CheckCreatedOrderItem(item);
            }

            // UNIT of Work
            //var tasks = dishList.Where(dish => dish.QuantityInStock == 0)
            //    .Select(dish => _orderItemRepo.UpdateCreatedOrderItemStatus(id, dish.Id, (int)OrderItemStates.Declined))
            //    .ToList();
            //await Task.WhenAll(tasks);
        }

        public async Task<OrderItemViewModel> CreateCustomerOrderItem(int orderId, OrderItemCreateModel orderItemCreateEntity)
        {
            orderItemCreateEntity = SetOrderIdForItem(orderId, orderItemCreateEntity);
            var orderItemEntity = await CreateItem(orderItemCreateEntity);
            await UpdateParentOrderStatusAndDate(orderItemEntity);
            var orderedDish = await _dishService.GetSingleDish(orderItemEntity.DishId);

            if (orderedDish.QuantityInStock != 0)
                return _mapper.Map<OrderItemViewModel>(orderItemEntity);

            orderItemEntity = await DeclineItem(orderItemEntity);
            return _mapper.Map<OrderItemViewModel>(orderItemEntity);
        }

        public async Task<OrderItem> GetSelectedOrderItem(int id)
        {
            return await _orderItemRepo.GetOrderItemById(id);
        }

        public async Task DeleteCustomerOrderItem(int itemId)
        {
            var orderItemEntity = await _orderItemRepo.GetOrderItemById(itemId);

            if(orderItemEntity == null)
                _loggerManager.LogError($"DeleteCustomerOrderItem(): Order item {itemId} was not found!");
            else
            {
                await _orderItemRepo.DeleteOrderItem(orderItemEntity);
                await UpdateParentOrderStatusAndDate(orderItemEntity);
                _loggerManager.LogInfo($"DeleteCustomerOrderItem(): Order item {orderItemEntity.Id} was deleted!");
            }
        }

        public async Task PrepareOrderItems(int orderId)
        {
            var orderItemsList = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
            foreach (var item in orderItemsList)
            {
                await PrepareOrderItem(item);
                //var task = orderItemsList.Select(orderItem => PrepareOrderItem(orderItem)).ToList();
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

        public async Task UpdateCustomerOrderItem(int itemId, OrderItemUpdateModel orderItemUpdateEntity)
        {
            if (OrderItemToBeServed(orderItemUpdateEntity))
            {
                await ServeOrderItem(itemId, orderItemUpdateEntity);
            }
            else
            {
                await UpdateOrderItemQuantity(itemId, orderItemUpdateEntity);
            }
        }

        private async Task CheckCreatedOrderItem(OrderItem orderItemEntity)
        {
            if (await OrderItemDishHasZeroStock(orderItemEntity))
            {
                _loggerManager.LogWarn($"CheckCreatedOrderItem(): Item '{orderItemEntity.Id}'" +
                    $" will have status declined! This item" +
                    $" currently has 0 stock. Status: '{(int)OrderItemStates.Declined}'");
                await _orderItemRepo.UpdatedAddedOrderItemStatus(orderItemEntity.Id, (int)OrderItemStates.Declined);
            }
        }

        private async Task<bool> OrderItemDishHasZeroStock(OrderItem orderItemEntity)
        {
            var itemDish = await _dishService.GetSingleDish(orderItemEntity.DishId);
            return itemDish.QuantityInStock == 0;
        }

        private bool OrderItemToBeServed(OrderItemUpdateModel orderItemUpdateEntity)
        {
            return orderItemUpdateEntity.IsServed == true;
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
            await _orderItemRepo.UpdatedAddedOrderItemStatus(orderItemEntity.Id, (int)OrderItemStates.Declined);
            _loggerManager.LogWarn($"DeclineItem(): Item '{orderItemEntity.Id}' for" +
                $" selected order was declined! Status: {(int)OrderItemStates.Declined}");
            return orderItemEntity;
        }

        private async Task<bool> ParentOrderHasAnyPreparingOrPreparedItems(int orderId)
        {
            //Review enums option
            var orderItemList = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
            return orderItemList.Any(x => x.OrderItemStatus == (int)OrderItemStates.Preparing)
                || orderItemList.Any(x => x.OrderItemStatus == (int)OrderItemStates.ReadyToServe);
        }

        private async Task UpdateParentOrderStatusAndDate(OrderItem orderItem)
        {
            if(await ParentOrderHasAnyPreparingOrPreparedItems(orderItem.OrderId))
            {
                await _orderRepo.UpdateOrderStatusAndDate(orderItem.OrderId, (int)OrderStates.Updated);
                _loggerManager.LogInfo($"UpdateParentOrderStatusAndDate(): Order '{orderItem.OrderId}' status was" +
                    $" updated! Status: {(int)OrderStates.Updated}");
            }
            else
            {
                await _orderRepo.UpdateOrderStatusAndDate(orderItem.OrderId, (int)OrderStates.Edited);
                _loggerManager.LogInfo($"UpdateParentOrderStatusAndDate(): Order '{orderItem.OrderId}' status was" +
                    $" updated! Status: {(int)OrderStates.Edited}");
            }
        }

        private static OrderItemCreateModel SetOrderIdForItem(int orderId, OrderItemCreateModel orderItemCreateEntity)
        {
            orderItemCreateEntity.OrderId = orderId;
            return orderItemCreateEntity;
        }

        private async Task<bool> OrderItemStatusIsNotPreparedAndAvalaibleInStock(OrderItem orderItemEntity)
        {
            var orderedDish = await _dishService.GetSingleDish(orderItemEntity.DishId);
            return (orderItemEntity.OrderItemStatus == (int)OrderItemStates.Created 
                || orderItemEntity.OrderItemStatus == (int)OrderItemStates.Declined
                || orderItemEntity.OrderItemStatus == (int)OrderItemStates.Updated)
                && orderedDish.QuantityInStock != 0;
        }

        private bool OrderItemIsPreparing(OrderItem orderItemEntity)
        {
            return orderItemEntity.OrderItemStatus == (int)OrderItemStates.Preparing;
        }

        private bool OrderItemIsAlreadyPreparingOrPrepared(OrderItem orderItemEntity)
        {
            return orderItemEntity.OrderItemStatus == (int)OrderItemStates.Preparing 
                || orderItemEntity.OrderItemStatus == (int)OrderItemStates.ReadyToServe
                || orderItemEntity.OrderItemStatus == (int)OrderItemStates.Served;
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
                    $"Order Item '{orderItemEntity.Id}' ordered dish" +
                    $" '{orderItemEntity.DishId}' has 0 stock!" +
                    $"Order Item won't be prepared.");
            await DeclineItem(orderItemEntity);
        }

        private async Task PrepareOrderItem(OrderItem orderItemEntity)
        {
            if (await OrderItemStatusIsNotPreparedAndAvalaibleInStock(orderItemEntity))
            {
                await PerformItemPreparation(orderItemEntity);
            }
            else if (OrderItemIsAlreadyPreparingOrPrepared(orderItemEntity))
            {
                _loggerManager.LogWarn($"PrepareOrderItem(): Item '{orderItemEntity.Id}' cannot be" +
                    $" preparing! This item is already " +
                    $"in 'Preparing' or in 'ReadyToServe' or in 'Served' status.");
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
            if (OrderItemIsPreparing(orderItemEntity))
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

        private bool OrderItemIsReady(OrderItem orderItemEntity)
        {
            return orderItemEntity.OrderItemStatus == (int)OrderItemStates.ReadyToServe;
        }

        private bool OrderItemIsNotPreparing(OrderItem orderItemEntity)
        {
            return orderItemEntity.OrderItemStatus == (int)OrderItemStates.Created
                || orderItemEntity.OrderItemStatus == (int)OrderItemStates.Declined;
        }

        private async Task ChangeOrderItemStatusToServed(OrderItem orderItemEntity)
        {
            await _orderItemRepo.UpdateOrderItemStatus(orderItemEntity, (int)OrderItemStates.Served);
            _loggerManager.LogInfo($"ChangeOrderItemStatusToServed(): Order item {orderItemEntity.Id} is" +
                $" served!");
        }

        private async Task ChangeOrderItemStatusToUpdated(OrderItem orderItemEntity)
        {
            await _orderItemRepo.UpdateOrderItemStatus(orderItemEntity, (int)OrderItemStates.Updated);
            _loggerManager.LogInfo($"ChangeOrderItemStatusToUpdated(): Order item {orderItemEntity.Id} is" +
                $" updated!");
        }

        private async Task UpdateOrderItemQuantity(int itemId, OrderItemUpdateModel orderItemUpdateEntity)
        {
            var orderItem = await _orderItemRepo.GetOrderItemById(itemId);
            if (OrderItemIsNotPreparing(orderItem))
            {
                await PartiallyUpdateItem(orderItem, orderItemUpdateEntity);
            }
            else
            {
                _loggerManager.LogWarn($"UpdateOrderItemQuantity(): Item '{itemId}' quantity hasn't been updated!" +
                    $"Order is already preparing or completed.");
            }
        }

        private async Task UpdateItem(OrderItem orderItem, OrderItemUpdateModel orderItemUpdateEntity)
        {
            var orderItemEntity = _mapper.Map<OrderItem>(orderItemUpdateEntity);
            orderItemEntity.Quantity = orderItem.Quantity;
            await _orderItemRepo.UpdateItemQtyAndFlag(orderItem.Id, orderItemEntity);
            _loggerManager.LogInfo($"UpdateItem(): Item '{orderItem.Id}' was updated. " +
                $"Quantity remaining the same, as it cannot be changed in prepared item.");
            await ChangeOrderItemStatusToServed(orderItem);
            _loggerManager.LogInfo($"UpdateItem(): Item '{orderItem.Id}' status was updated. Status: 75.");
            await UpdateParentOrderStatusAndDate(orderItem);
        }

        private async Task PartiallyUpdateItem(OrderItem orderItem, OrderItemUpdateModel orderItemUpdateEntity)
        {
            var orderItemEntity = _mapper.Map<OrderItem>(orderItemUpdateEntity);
            orderItemEntity.IsServed = false;
            await _orderItemRepo.UpdateItemQtyAndFlag(orderItem.Id, orderItemEntity);
            _loggerManager.LogInfo($"PartiallyUpdateItem(): Item '{orderItem.Id}' was partially updated. IsServed" +
                $" flag change was rollbacked or initially was false.");
            await ChangeOrderItemStatusToUpdated(orderItem);
            _loggerManager.LogInfo($"PartiallyUpdateItem(): Item '{orderItem.Id}' status was updated. Status: 70.");
            await UpdateParentOrderStatusAndDate(orderItem);
        }

        private async Task ServeOrderItem(int itemId, OrderItemUpdateModel orderItemUpdateEntity)
        {
            var orderItem = await _orderItemRepo.GetOrderItemById(itemId);
            if (OrderItemIsReady(orderItem))
            {
                await UpdateItem(orderItem, orderItemUpdateEntity);
            }
            else if(OrderItemIsNotPreparing(orderItem))
            {
                _loggerManager.LogWarn($"ServeOrderItem(): Item '{orderItem.Id}'" +
                    $" cannot be served! This item is not " +
                    $"in 'ReadyToServe' status.");
                await PartiallyUpdateItem(orderItem, orderItemUpdateEntity);
            }
            else
            {
                _loggerManager.LogWarn($"ServeOrderItem(): Item '{orderItem.Id}'" +
                    $" cannot be served, nor quantity of it can be updated! This item is not " +
                    $"in 'ReadyToServe' status or it is preparing/completed.");
            }
        }
    }
}
