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
    [Route("api/order")]
    [ApiController]
    public class APIOrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IOrderService _orderService;

        public APIOrderController(IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IMapper mapper, ILoggerManager loggerManager, IOrderService orderService)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _orderService = orderService;
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

                var exists = _orderRepo.CheckOrderUniqueness(order.OrderName);

                if (exists)
                {
                    _loggerManager.LogError($"CreateOrder(): Post new order is failed, same order already exists.");
                    return BadRequest("Order with same name already exists!");
                }
                else
                {
                    var createdOrder = await _orderService.CreateCustomerOrder(order);
                    return CreatedAtRoute("OrderById", new { id = createdOrder.Id }, createdOrder);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateOrder() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}/item")]
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


    }
}
