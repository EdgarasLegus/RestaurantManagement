using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;

namespace RestaurantManagement.WebApp.Controllers
{
    [Route("api/dish")]
    [ApiController]
    public class APIDishController : ControllerBase
    {
        private readonly IDishRepo _dishRepo;
        private readonly IDishProductRepo _dishProductRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public APIDishController(IDishRepo dishRepo, IDishProductRepo dishProductRepo, IMapper mapper, ILoggerManager loggerManager)
        {
            _dishRepo = dishRepo;
            _dishProductRepo = dishProductRepo;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetDishes()
        {
            try
            {
                var dishes = await _dishRepo.GetDishes();
                _loggerManager.LogInfo($"GetDishes() method returned existing dishes list.");
                var dishResult = _mapper.Map<IEnumerable<DishViewModel>>(dishes);
                return Ok(dishResult);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetDishes() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}", Name = "DishById")]
        public async Task<IActionResult> GetDishById(int id)
        {
            try
            {
                var dish = await _dishRepo.GetDishById(id);

                if (dish == null)
                {
                    _loggerManager.LogError($"Dish with id: {id} is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetDishById() returned dish with id: {id}");
                    var dishResult = _mapper.Map<DishViewModel>(dish);
                    return Ok(dishResult);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetDishById() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("{id}/product")]
        public async Task<IActionResult> GetDishProducts(int id)
        {
            try
            {
                var dish = await _dishRepo.GetDishWithProducts(id);
                if (dish == null)
                {
                    _loggerManager.LogError($"GetDishWithProducts(): Dish with id: {id}, is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetDishWithProducts() method returned dish with products for id: {id}");
                    var dishResult = _mapper.Map<DishViewModel>(dish);
                    return Ok(dishResult);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetDishWithProducts() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
