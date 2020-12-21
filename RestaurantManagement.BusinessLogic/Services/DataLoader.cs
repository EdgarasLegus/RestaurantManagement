using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using RestaurantManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Contracts.Entities;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DataLoader : IDataLoader
    {
        private readonly IStaffService _staffService;
        private readonly IRestaurantTablesService _restaurantTableService;
        private readonly IProductService _productService;
        private readonly IDishService _dishService;
        private readonly IDishRepo _dishRepo;
        private readonly IDishProductService _dishProductService;
        private readonly IDishProductRepo _dishProductRepo;
        private readonly IPersonRoleService _personRoleService;
        private readonly IUnitOfWork _unitOfWork;



        public DataLoader(IStaffService staffService, IRestaurantTablesService restaurantTablesService, 
            IProductService productService, IDishService dishService, IDishRepo dishRepo,
            IDishProductService dishProductService,
            /*IDishProductRepo dishProductRepo,*/ IPersonRoleService personRoleService,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _staffService = staffService;
            _restaurantTableService = restaurantTablesService;
            _productService = productService;
            _dishService = dishService;
            _dishRepo = dishRepo;
            _dishProductService = dishProductService;
            _dishProductRepo = unitOfWork.DishProductRepo;
            _personRoleService = personRoleService;
        }

        public async Task LoadInitialData()
        {
            //await _dishRepo.InsertInitialDishes(_dishService.GetInitialDishes());
            //await _dishProductRepo.InsertInitialDishProducts(_dishProductService.GetInitialDishProducts());
            await LoadRoles();
            await LoadStaff();
            await LoadTables();
            await LoadProducts();
            await LoadDishes();
            await LoadDishProducts();

            //await _context.SaveChangesAsync();
        }

        private async Task LoadRoles()
        {
            await _unitOfWork.GetRepository<PersonRole>().InsertInitialEntity(_personRoleService.GetInitialPersonRoles());
        }

        private async Task LoadStaff()
        {
            await _unitOfWork.GetRepository<Staff>().InsertInitialEntity(_staffService.GetInitialStaff());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadTables()
        {
            await _unitOfWork.GetRepository<RestaurantTable>().InsertInitialEntity(_restaurantTableService.GetInitialRestaurantTables());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadProducts()
        {
            await _unitOfWork.GetRepository<Product>().InsertInitialEntity(_productService.GetInitialProducts());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadDishes()
        {
            await _dishRepo.InsertInitialDishes(_dishService.GetInitialDishes());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadDishProducts()
        {
            await _dishProductRepo.InsertInitialDishProducts(_dishProductService.GetInitialDishProducts());
            //await _context.SaveChangesAsync();
        }
    }
}
