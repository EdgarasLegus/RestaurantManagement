﻿using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IProductService
    {
        List<Product> GetInitialProducts();
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int id);
        Task<ProductViewModel> CreateProduct(ProductCreateModel productCreateModel);
    }
}
