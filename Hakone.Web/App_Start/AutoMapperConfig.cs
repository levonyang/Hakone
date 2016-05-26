using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hakone.Domain;
using Hakone.Web.Models;

namespace Hakone.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<RegisterModel, User>();
            AutoMapper.Mapper.CreateMap<User, EditUserInfoModel>();
            AutoMapper.Mapper.CreateMap<Shop, ShopFormModel>();
            AutoMapper.Mapper.CreateMap<Product, ProductFormModel>();
            AutoMapper.Mapper.CreateMap<ProductES, Product>();
            AutoMapper.Mapper.CreateMap<ShopES, Shop>();
        }
    }
}