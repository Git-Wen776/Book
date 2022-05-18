using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.AutoMapperConfig
{
    public class BaseProfile<TModel,TDto>:Profile
    {
        public BaseProfile()
        {
            CreateMap<TModel,TDto>().ReverseMap();
        }
    }
}
