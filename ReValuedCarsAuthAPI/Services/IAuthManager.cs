using ReValuedCarsAuthAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReValuedCarsAuthAPI.Services
{
    public interface IAuthManager
    {
        Task<dynamic> AddUserAsync(User user);

        string AuthUsers(LoginModel user);
    }
}
