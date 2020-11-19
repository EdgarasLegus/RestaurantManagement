using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;

namespace RestaurantManagement.WebApp.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class APIOrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public APIOrderController(IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IMapper mapper, ILoggerManager loggerManager,
            IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderRepo.GetOrders();
                _loggerManager.LogInfo($"GetOrders() method returned all orders list.");
                var orderResult = _mapper.Map<IEnumerable<OrderViewModel>>(orders);
                return Ok(orderResult);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetOrders() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}", Name = "OrderById")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderRepo.GetOrderById(id);

                if (order == null)
                {
                    _loggerManager.LogError($"Order with id: {id} is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetOrderById() returned order with id: {id}");
                    var orderResult = _mapper.Map<OrderViewModel>(order);
                    return Ok(orderResult);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetOrderById() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}/items/{itemId}", Name = "OrderItemById")]
        public async Task<IActionResult> GetOrderItemById(int id, int itemId)
        {
            try
            {
                var order = await _orderRepo.GetOrderById(id);
                var orderItem = await _orderItemService.GetSelectedOrderItem(itemId);
                if (order == null)
                {
                    _loggerManager.LogError($"Order with id: {id} is not found in database.");
                    return NotFound();
                }
                else if (orderItem == null)
                {
                    _loggerManager.LogError($"Order item with id: {itemId} is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetOrderItemById() returned order item with id: {itemId}");
                    var orderResult = _mapper.Map<OrderItemViewModel>(orderItem);
                    return Ok(orderResult);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetOrderItemById() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateModel order)
        {
            try
            {
                if (order == null)
                {
                    _loggerManager.LogError($"CreateOrder(): Post new order is failed, it is empty.");
                    return BadRequest("Order is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid order model object");
                }

                var createdOrder = await _orderService.CreateCustomerOrder(order);
                return CreatedAtRoute("OrderById", new { id = createdOrder.Id }, createdOrder);
                
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateOrder() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetOrderItems(int id)
        {
            try
            {
                var order = await _orderRepo.GetOrderWithItems(id);
                if (order == null)
                {
                    _loggerManager.LogError($"GetOrderWithItems(): Order with id: {id}, is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetOrderWithItems() method returned order with items for id: {id}");
                    var orderResult = _mapper.Map<OrderViewModel>(order);
                    return Ok(orderResult);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetOrderWithItems() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            try
            {
                //ASK
                await _orderService.DeleteCustomerOrder(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"DeleteOrder() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddOrderItem(int id, [FromBody] OrderItemCreateModel orderItem)
        {
            try
            {
                var existingOrder = await _orderService.GetExistingOrder(id);
                if (existingOrder == null)
                {
                    _loggerManager.LogError($"AddOrderItem(): Post new order item is failed, order does not exist.");
                    return BadRequest("Order is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid order model object");
                }

                var createdOrderItem = await _orderItemService.CreateCustomerOrderItem(id, orderItem);
                return CreatedAtRoute("OrderItemById", new { id, itemId = createdOrderItem.Id }, createdOrderItem);

            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateOrder() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> DeleteOrderItem([FromRoute] int id, int itemId)
        {
            try
            {
                //ASK
                await _orderItemService.DeleteCustomerOrderItem(itemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"DeleteOrderItem() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateModel orderUpdateEntity)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid order update model object");
                }

                await _orderService.UpdateCustomerOrder(orderUpdateEntity, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"UpdateOrderItem() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
