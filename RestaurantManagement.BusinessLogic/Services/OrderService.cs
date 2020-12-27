using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDishService _dishService;
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Order> _orderRepo;

        public OrderService(IDishService dishService, IOrderItemService orderItemService,
            IMapper mapper, ILoggerManager loggerManager, IUnitOfWork unitOfWork)
        {
            _dishService = dishService;
            _orderItemService = orderItemService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;

            _orderRepo = _unitOfWork.GetRepository<Order>();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _orderRepo.Get();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _orderRepo.GetFirstOrDefault(x => x.Id == id);
        }

        public async Task<Order> GetOrderWithItems(int id)
        {
            var order = await _orderRepo.GetFirstOrDefault(o => o.Id == id, x => x.Include(x => x.OrderItem));
            return order;
        }

        public async Task AddOrder(Order orderEntity)
        {
            await _orderRepo.Add(orderEntity);
            await _unitOfWork.Commit();
        }

        public async Task<OrderViewModel> CreateCustomerOrder(OrderCreateModel orderCreateEntity)
        {
            var orderEntity = await Create(orderCreateEntity);
            var dishIdsList = await _orderItemService.GetDishesIdsByOrderId(orderEntity.Id);
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

        public async Task DeleteCustomerOrder(int id)
        {
            var orderEntity = await GetOrderById(id);
            _orderRepo.Delete(orderEntity);
            await _unitOfWork.Commit();
            _loggerManager.LogInfo($"DeleteCustomerOrder(): Order '{orderEntity.OrderName}' was deleted!");
        }

        public async Task UpdateCreatedOrder(int id, Order orderEntity)
        {
            var existingOrder = await _orderRepo.GetFirstOrDefault(x => x.Id == id);

            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.CreatedDate = orderEntity.CreatedDate;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = orderEntity.OrderStatus;
            existingOrder.OrderItem = orderEntity.OrderItem;
            existingOrder.IsPreparing = orderEntity.IsPreparing;
            existingOrder.IsReady = orderEntity.IsReady;

            await _unitOfWork.Commit();
        }

        public async Task UpdateOrderStatusAndDate(int id, int status)
        {
            var existingOrder = await _orderRepo.GetFirstOrDefault(x => x.Id == id);
            existingOrder.OrderStatus = status;
            existingOrder.ModifiedDate = DateTime.Now;
            await _unitOfWork.Commit();
        }

        public async Task UpdateExistingOrder(int id, Order orderEntity, int status)
        {
            var existingOrder = await _orderRepo.GetFirstOrDefault(x => x.Id == id);
            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = status;
            existingOrder.IsPreparing = orderEntity.IsPreparing;
            existingOrder.IsReady = orderEntity.IsReady;
            await _unitOfWork.Commit();
        }

        public async Task UpdateOrderNameAndStatus(int id, Order orderEntity, int status)
        {
            var existingOrder = await _orderRepo.GetFirstOrDefault(x => x.Id == id);
            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = status;
            await _unitOfWork.Commit();
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


        // Review
        private bool OrderIsJustUpdated(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == false && orderUpdateEntity.IsReady == false;
        }

        private bool OrderIsToBePrepared(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == true && orderUpdateEntity.IsReady == false;
        }

        private bool OrderIsToBeReady(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == false && orderUpdateEntity.IsReady == true;
        }

        private bool OrderHasSomeItemsWithZeroStock(List<Dish> orderedDishList)
        {
            return orderedDishList.Any(x => x.QuantityInStock == 0) 
                && orderedDishList.Any(x => x.QuantityInStock != 0);
        }

        private bool OrderHasAllItemsWithZeroStock(List<Dish> orderedDishList)
        {
            return orderedDishList.All(x => x.QuantityInStock == 0);
        }

        private async Task ChangeCreatedOrderStatus(Order orderEntity)
        {
            var createdOrder = await GetOrderById(orderEntity.Id);
            await UpdateCreatedOrder(createdOrder.Id, orderEntity);
            await _orderItemService.UpdateCreatedOrderItemsStatuses(createdOrder.Id);
        }

        private async Task<Order> Create(OrderCreateModel orderCreateEntity)
        {
            var orderEntity = _mapper.Map<Order>(orderCreateEntity);
            await AddOrder(orderEntity);
            _loggerManager.LogInfo($"CreateCustomerOrder(): New order '{orderEntity.OrderName}' is" +
                $" successfully created! Status: 10");
            return orderEntity;
        }

        private async Task<Order> PartiallyDecline(Order orderEntity, List<Dish> orderedDishList)
        {
            orderEntity.OrderStatus = (int)OrderStates.PartiallyDeclined;
            await ChangeCreatedOrderStatus(orderEntity);
            _loggerManager.LogWarn($"CreateCustomerOrder(): Order '{orderEntity.OrderName}' was" +
                $" partially declined! Status: 20");
            return orderEntity;
        }

        private async Task<Order> Decline(Order orderEntity, List<Dish> orderedDishList)
        {
            orderEntity.OrderStatus = (int)OrderStates.Declined;
            await ChangeCreatedOrderStatus(orderEntity);
            _loggerManager.LogWarn($"CreateCustomerOrder(): Order '{orderEntity.OrderName}' was" +
                $" declined! Status: 30");
            return orderEntity;
        }

        private async Task Update(OrderUpdateModel orderUpdateEntity, int id)
        {
            if (AllModelFlagsAreFalse(orderUpdateEntity))
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
            await UpdateOrderStatusAndDate(orderId, (int)OrderStates.Preparing);
            _loggerManager.LogInfo($"ChangePreparingOrderStatus(): Order {orderId} is" +
                $" in preparing status.");
        }

        private async Task ChangeReadyOrderStatus(int orderId)
        {
            await UpdateOrderStatusAndDate(orderId, (int)OrderStates.ReadyToServe);
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
            var order = await GetOrderById(orderId);
            return order.IsPreparing == false && order.IsReady == false;
        }

        private bool AllModelFlagsAreFalse(OrderUpdateModel orderUpdateEntity)
        {
            return orderUpdateEntity.IsPreparing == false 
                && orderUpdateEntity.IsReady == false;
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
            await UpdateExistingOrder(id, orderEntity, (int)OrderStates.Edited);
            _loggerManager.LogInfo($"Update(): Order '{id}' was updated!");
        }

        private async Task AllowPartialUpdate(OrderUpdateModel orderUpdateEntity, int id)
        {
            var orderEntity = _mapper.Map<Order>(orderUpdateEntity);
            await UpdateOrderNameAndStatus(id, orderEntity, (int)OrderStates.Edited);
            _loggerManager.LogWarn($"AllowPartialUpdate(): Order '{id}' was partially updated. Flags update was rollbacked.");
        }

    }
}
