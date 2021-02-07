using RestaurantManagement.Interfaces.Services;
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
        private readonly IDishProductService _dishProductService;
        private readonly IPersonRoleService _personRoleService;
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;
        private readonly IUnitOfWork _unitOfWork;



        public DataLoader(IStaffService staffService, IRestaurantTablesService restaurantTablesService, 
            IProductService productService, IDishService dishService, IDishProductService dishProductService,
            IPersonRoleService personRoleService, IUnitOfMeasurementService unitOfMeasurementService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _staffService = staffService;
            _restaurantTableService = restaurantTablesService;
            _productService = productService;
            _dishService = dishService;
            _dishProductService = dishProductService;
            _personRoleService = personRoleService;
            _unitOfMeasurementService = unitOfMeasurementService;
        }

        public async Task LoadInitialData()
        {
            await LoadUnits();
            await LoadRoles();
            await LoadStaff();
            await LoadTables();
            await LoadProducts();
            await LoadDishes();
            await LoadDishProducts();
            //await _context.SaveChangesAsync();
        }

        private async Task LoadUnits()
        {
            await _unitOfWork.GetRepository<UnitOfMeasurement>().InsertInitialEntity(_unitOfMeasurementService.GetInitialUnitsOfMeasurement());
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
            await _unitOfWork.GetRepository<Dish>().InsertInitialEntity(_dishService.GetInitialDishes());
            //await _context.SaveChangesAsync();
        }

        private async Task LoadDishProducts()
        {
            await _unitOfWork.GetRepository<DishProduct>().InsertInitialEntity(_dishProductService.GetInitialDishProducts());
            //await _context.SaveChangesAsync();
        }
    }
}
