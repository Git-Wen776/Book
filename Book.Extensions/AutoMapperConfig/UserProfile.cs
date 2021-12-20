using Book.Models;
using Book.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.AutoMapperConfig
{
    public class UserProfile:BaseProfile<User,UserDto>
    {
    }
}
