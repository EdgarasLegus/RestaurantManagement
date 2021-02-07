using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product> _productRepo;

        public ProductService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options, IUnitOfWork unitOfWork)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
            _unitOfWork = unitOfWork;
            _productRepo = _unitOfWork.GetRepository<Product>();
        }

        public List<Product> GetInitialProducts()
        {
            var initialProductsFile = _options.InitialProducts;
            var fileParts = _logicHandler.FileReader(initialProductsFile);
            var productList = new List<Product>();

            foreach (var product in fileParts.Select(subList => new Product()
            {
                Id = int.Parse(subList[0]),
                ProductName = subList[1],
                StockAmount = Convert.ToDecimal(subList[2]),
                UnitOfMeasurementId = int.Parse(subList[3])
            }).Where(product => productList.All(x => x.ProductName != product.ProductName)))
            {
                productList.Add(product);
            }
            return productList;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _productRepo.Get();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepo.GetFirstOrDefault(x => x.Id == id);
        }

        public async Task AddProduct(Product productEntity)
        {
            await _productRepo.Add(productEntity);
            await _unitOfWork.Commit();
        }
    }
}
