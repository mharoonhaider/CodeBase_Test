using Codebase_Test.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Codebase_Test.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
        }

        public DbSet<UserModel> UserInfo { get; set; }
    }
}
