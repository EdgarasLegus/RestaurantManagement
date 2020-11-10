using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepo _orderItemRepo;

        public OrderItemService(IOrderItemRepo orderItemRepo)
        {
            _orderItemRepo = orderItemRepo;
        }

        public async Task UpdateOrderItemsStatuses(int id, List<Dish> dishList)
        {
            foreach (var dish in dishList)
            {
                if (dish.QuantityInStock == 0)
                {
                    await _orderItemRepo.UpdateOrderDishStatus(id, dish.Id, (int)OrderItemStates.Declined);
                }
            }
        }
    }
}
