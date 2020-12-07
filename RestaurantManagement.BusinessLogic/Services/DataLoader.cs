using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using RestaurantManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.Interfaces;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DataLoader : IDataLoader
    {
        private readonly IStaffService _staffService;
        private readonly IStaffRepo _staffRepo;
        private readonly IRestaurantTablesService _restaurantTableService;
        private readonly IRestaurantTableRepo _restaurantTableRepo;
        private readonly IProductService _productService;
        private readonly IProductRepo _productRepo;
        private readonly IDishService _dishService;
        private readonly IDishRepo _dishRepo;
        private readonly IDishProductService _dishProductService;
        private readonly IDishProductRepo _dishProductRepo;
        private readonly IPersonRoleService _personRoleService;
        private readonly IPersonRoleRepo _personRoleRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DataLoader(IStaffService staffService, IStaffRepo staffRepo, IRestaurantTablesService restaurantTablesService,
            IRestaurantTableRepo restaurantTableRepo, IProductService productService, IProductRepo productRepo, IDishService dishService, IDishRepo dishRepo,
            IDishProductService dishProductService, /*IDishProductRepo dishProductRepo,*/ IPersonRoleService personRoleService, IPersonRoleRepo personRoleRepo, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _staffService = staffService;
            _staffRepo = staffRepo;
            _restaurantTableService = restaurantTablesService;
            _restaurantTableRepo = restaurantTableRepo;
            _productService = productService;
            _productRepo = productRepo;
            _dishService = dishService;
            _dishRepo = dishRepo;
            _dishProductService = dishProductService;
            _dishProductRepo = unitOfWork.DishProductRepo;
            _personRoleService = personRoleService;
            _personRoleRepo = personRoleRepo;
        }

        public async Task LoadInitialData()
        {
            //await _staffRepo.InsertInitialStaff(_staffService.GetInitialStaff());
            //await _restaurantTableRepo.InsertInitialRestaurantTables(_restaurantTableService.GetInitialRestaurantTables());
            //await _productRepo.InsertInitialProducts(_productService.GetInitialProducts());
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
            await _personRoleRepo.InsertInitialPersonRoles(_personRoleService.GetInitialPersonRoles());
        }

        private async Task LoadStaff()
        {
            await _staffRepo.InsertInitialStaff(_staffService.GetInitialStaff());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadTables()
        {
            await _restaurantTableRepo.InsertInitialRestaurantTables(_restaurantTableService.GetInitialRestaurantTables());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadProducts()
        {
            await _productRepo.InsertInitialProducts(_productService.GetInitialProducts());
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
