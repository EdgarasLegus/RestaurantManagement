using AutoMapper;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.WebApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffViewModel>();
            CreateMap<StaffCreateModel, Staff>();
            CreateMap<StaffUpdateModel, Staff>();

            CreateMap<Order, OrderViewModel>().ForMember(x => x.OrderItems, s => s.MapFrom(m => m.OrderItem));
            CreateMap<OrderItem, OrderItemViewModel>();

            CreateMap<OrderCreateModel, Order>().ForMember(x => x.OrderItem, s => s.MapFrom(m => m.OrderItems));
            CreateMap<OrderItemCreateModel, OrderItem>();

            CreateMap<OrderUpdateModel, Order>().ForMember(x => x.OrderItem, s => s.MapFrom(m => m.OrderItems));
            CreateMap<OrderItemUpdateModel, OrderItem>();
            
            CreateMap<OrderPartiallyDeclinedModel, Order>().ForMember(x => x.OrderItem, s => s.MapFrom(m => m.OrderItems));
            CreateMap<OrderItemPartiallyDeclinedModel, OrderItem>();

            CreateMap<OrderCreateModel, OrderPartiallyDeclinedModel>().ForMember(x => x.OrderItems, s => s.MapFrom(m => m.OrderItems));
            CreateMap<OrderItemCreateModel, OrderItemPartiallyDeclinedModel>();

            CreateMap<OrderDeclinedModel, Order>().ForMember(x => x.OrderItem, s => s.MapFrom(m => m.OrderItems));
            CreateMap<OrderItemDeclinedModel, OrderItem>();

            CreateMap<OrderCreateModel, OrderDeclinedModel>().ForMember(x => x.OrderItems, s => s.MapFrom(m => m.OrderItems));
            CreateMap<OrderItemCreateModel, OrderItemDeclinedModel>();

            CreateMap<Dish, DishViewModel>().ForMember(x => x.DishProduct, y => y.MapFrom(z => z.DishProduct));
            CreateMap<DishProduct, DishProductViewModel>();

            CreateMap<Product, ProductViewModel>();

        }
    }
}
