using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Services;

namespace RestaurantManagement.WebApp.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class APIProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ILoggerManager _loggerManager;

        public APIProductController(IMapper mapper, IProductService productService, ILoggerManager loggerManager)
        {
            _mapper = mapper;
            _productService = productService;
            _loggerManager = loggerManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                _loggerManager.LogInfo($"GetProducts() method returned all products list.");
                var result = _mapper.Map<IEnumerable<ProductViewModel>>(products);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetProducts() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null)
                {
                    _loggerManager.LogError($"Product with id: {id} is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetProductById() returned order with id: {id}");
                    var result = _mapper.Map<ProductViewModel>(product);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetProductById() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModel productCreateModel)
        {
            try
            {
                if (productCreateModel == null)
                {
                    _loggerManager.LogError($"CreateProduct(): Post of new product is failed, it is empty.");
                    return BadRequest("Product is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid product model object");
                }
                var createdProduct = await _productService.CreateProduct(productCreateModel);
                return CreatedAtRoute("ProductById", new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateProduct() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
