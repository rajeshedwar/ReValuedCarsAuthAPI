using ReValuedCarsAuthAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReValuedCarsAuthAPI.Infrastructure
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
