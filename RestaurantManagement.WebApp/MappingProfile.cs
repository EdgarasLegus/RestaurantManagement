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
            CreateMap<Staff, StaffModel>();
            CreateMap<StaffCreateModel, Staff>();
            CreateMap<StaffUpdateModel, Staff>();

            CreateMap<Order, OrderModel>();
            CreateMap<OrderCreateModel, Order>();

            CreateMap<OrderItem, OrderItemModel>();
            CreateMap<OrderItemCreateModel, OrderItem>();
        }
    }
}
