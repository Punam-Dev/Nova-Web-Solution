using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace NovaWebSolution.Models
{
    public class AppDbContext : DbContext
    {
        static string DevMode = string.Empty;
        public AppDbContext() : base(ConnectionString())
        {
            DevMode = WebConfigurationManager.AppSettings["DevMode"];
        }
        private static string ConnectionString()
        {
            if (DevMode == "1")
            {
                return "Name=DbCon";
            }
            else
            {
                return "Name=DbConNova";
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure default schema
            if (DevMode == "1")
            {
                modelBuilder.HasDefaultSchema("dbo");
            }
            else
            {
                modelBuilder.HasDefaultSchema("novaweb");
            }
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Users> users { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<FormQuery> FormQuery { get; set; }
        public DbSet<UserLogInDetails> UserLogInDetails { get; set; }
    }
}