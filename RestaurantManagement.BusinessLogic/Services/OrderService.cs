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

        public OrderService(IDishService dishService, IOrderItemService orderItemService, IOrderRepo orderRepo,
            IOrderItemRepo orderItemRepo, IMapper mapper, ILoggerManager loggerManager)
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

            if (OrderHasSomeItemsWithZeroStock(orderedDishList))
            {
                orderEntity = await PartiallyDecline(orderEntity, orderedDishList);
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            else if (OrderHasAllItemsWithZeroStock(orderedDishList))
            {
                orderEntity = await Decline(orderEntity, orderedDishList);
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            else
            {
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
        }

        public async Task<Order> GetExistingOrder(int id)
        {
            return await _orderRepo.GetOrderWithItems(id);
        }

        public async Task DeleteCustomerOrder(int id)
        {
            var orderEntity = await _orderRepo.GetOrderById(id);
            await _orderRepo.DeleteOrder(orderEntity);
            _loggerManager.LogInfo($"DeleteCustomerOrder(): Order '{orderEntity.OrderName}' was deleted!");
        }

        public async Task UpdateCustomerOrder(OrderUpdateModel orderUpdateEntity, int orderId)
        {
            if (OrderIsJustUpdated(orderUpdateEntity))
            {
                await Update(orderUpdateEntity, orderId);
            }
            else if (OrderIsToBePrepared(orderUpdateEntity))
            {
                await Prepare(orderUpdateEntity, orderId);
            }
            else if (OrderIsToBeReady(orderUpdateEntity))
            {
                await ReadyToServe(orderUpdateEntity, orderId);
            }
            else
            {
                _loggerManager.LogError($"UpdateCustomerOrder(): Order '{orderId}' cannot" +
                    $" be preparing and ready in same time! " +
                    $" No changes were applied.");
            }
        }

        // can be moved to Order entity
        private bool OrderIsJustUpdated(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == false && orderUpdateEntity.IsReady == false;
        }

        // can be moved to Order entity
        private bool OrderIsToBePrepared(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == true && orderUpdateEntity.IsReady == false;
        }

        // can be moved to Order entity
        private bool OrderIsToBeReady(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == false && orderUpdateEntity.IsReady == true;
        }

        private bool OrderHasSomeItemsWithZeroStock(List<Dish> orderedDishList)
        {
            return orderedDishList.Any(x => x.QuantityInStock == 0) && orderedDishList.Any(x => x.QuantityInStock != 0);
        }

        private bool OrderHasAllItemsWithZeroStock(List<Dish> orderedDishList)
        {
            return orderedDishList.All(x => x.QuantityInStock == 0);
        }

        private async Task ChangeCreatedOrderStatus(Order orderEntity, List<Dish> orderedDishList)
        {
            var createdOrder = await _orderRepo.GetOrderById(orderEntity.Id);
            await _orderRepo.UpdateOrder(createdOrder.Id, orderEntity);
            await _orderItemService.UpdateCreatedOrderItemsStatuses(createdOrder.Id, orderedDishList);
        }

        private async Task<Order> Create(OrderCreateModel orderCreateEntity)
        {
            var orderEntity = _mapper.Map<Order>(orderCreateEntity);
            await _orderRepo.AddOrder(orderEntity);
            _loggerManager.LogInfo($"CreateCustomerOrder(): New order '{orderEntity.OrderName}' is" +
                $" successfully created! Status: 10");
            return orderEntity;
        }

        private async Task<Order> PartiallyDecline(Order orderEntity, List<Dish> orderedDishList)
        {
            orderEntity.OrderStatus = (int)OrderStates.PartiallyDeclined;
            await ChangeCreatedOrderStatus(orderEntity, orderedDishList);
            _loggerManager.LogWarn($"CreateCustomerOrder(): Order '{orderEntity.OrderName}' was" +
                $" partially declined! Status: 20");
            return orderEntity;
        }

        private async Task<Order> Decline(Order orderEntity, List<Dish> orderedDishList)
        {
            orderEntity.OrderStatus = (int)OrderStates.Declined;
            await ChangeCreatedOrderStatus(orderEntity, orderedDishList);
            _loggerManager.LogWarn($"CreateCustomerOrder(): Order '{orderEntity.OrderName}' was" +
                $" declined! Status: 30");
            return orderEntity;
        }

        private async Task Update(OrderUpdateModel orderUpdateEntity, int id)
        {
            if (await AllModelFlagsAreFalse(orderUpdateEntity))
            {
                await AllowPartialUpdate(orderUpdateEntity, id);
            }
            else
            {
                await AllowUpdate(orderUpdateEntity, id);
            }
        }

        private async Task Prepare(OrderUpdateModel orderUpdateEntity, int orderId)
        {
            await Update(orderUpdateEntity, orderId);
            await _orderItemService.PrepareOrderItems(orderId);
            await ChangePreparingOrderStatus(orderId);
        }

        private async Task ChangePreparingOrderStatus(int orderId)
        {
            await _orderRepo.UpdateOrderStatusAndDate(orderId, (int)OrderStates.Preparing);
            _loggerManager.LogInfo($"ChangePreparingOrderStatus(): Order {orderId} is" +
                $" in preparing status.");
        }

        private async Task ChangeReadyOrderStatus(int orderId)
        {
            await _orderRepo.UpdateOrderStatusAndDate(orderId, (int)OrderStates.ReadyToServe);
            _loggerManager.LogInfo($"ChangeReadyOrderStatus(): Order {orderId} is" +
                $" in ready to serve status.");
        }

        private async Task ReadyToServe(OrderUpdateModel orderUpdateEntity, int orderId)
        {
            if (await PreparingAndReadyFlagsAreFalse(orderId))
            {
                _loggerManager.LogError($"ReadyToServe(): Order {orderId} cannot be" +
                    $" put in 'Ready to Serve' status." +
                    $" No items have been prepared or served.");
            }
            else
            {
                await AllowReadyToServe(orderUpdateEntity, orderId);
            }
        }

        private async Task<bool> PreparingAndReadyFlagsAreFalse(int orderId)
        {
            var order = await _orderRepo.GetOrderById(orderId);
            return order.IsPreparing == false && order.IsReady == false;
        }

        // should not be async
        private async Task<bool> AllModelFlagsAreFalse(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == false && orderUpdateEntity.IsReady == false;
        }

        private async Task AllowReadyToServe(OrderUpdateModel orderUpdateEntity, int orderId)
        {
            await Update(orderUpdateEntity, orderId);
            await _orderItemService.ReadyToServeOrderItems(orderId);
            await ChangeReadyOrderStatus(orderId);
        }

        private async Task AllowUpdate(OrderUpdateModel orderUpdateEntity, int id)
        {
            var orderEntity = _mapper.Map<Order>(orderUpdateEntity);
            await _orderRepo.UpdateExistingOrder(id, orderEntity, (int)OrderStates.Edited);
            _loggerManager.LogInfo($"Update(): Order '{id}' was updated!");
        }

        private async Task AllowPartialUpdate(OrderUpdateModel orderUpdateEntity, int id)
        {
            var orderEntity = _mapper.Map<Order>(orderUpdateEntity);
            await _orderRepo.UpdateOrderNameAndStatus(id, orderEntity, (int)OrderStates.Edited);
            _loggerManager.LogWarn($"AllowPartialUpdate(): Order '{id}' was partially updated. Flags update was rollbacked.");
        }

    }
}
