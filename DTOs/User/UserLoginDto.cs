using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.DTOs.User
{
    public class UserLoginDto
    {
        public string UserName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}