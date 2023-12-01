using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreEL.Mappings
{
    public class Maps : Profile
    {

        public Maps()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryProductProperty, CategoryProductPropertyDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductDiscount, ProductDiscountDTO>().ReverseMap();
            CreateMap<ProductPicture, ProductPictureDTO>().ReverseMap();
            CreateMap<ProductProperty, ProductPropertyDTO>().ReverseMap();
            CreateMap<FeaturedPartModel, FeaturedPartViewModel>().ReverseMap();
            CreateMap<ContactMessage, ContactMessageDTO>().ReverseMap();
        }
    }
}
