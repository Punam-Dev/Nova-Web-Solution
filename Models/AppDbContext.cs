using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DbCon")
        { }
        public DbSet<Users> users { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<FormQuery> FormQuery { get; set; }
        public DbSet<UserLogInDetails> UserLogInDetails { get; set; }
    }
}