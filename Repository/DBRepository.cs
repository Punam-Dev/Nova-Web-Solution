using NovaWebSolution.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace NovaWebSolution.Repository
{
    public class DBRepository : IDBRepository
    {
        private readonly AppDbContext _appDbContext;
        public DBRepository(AppDbContext appDbContex)
        {
            _appDbContext = appDbContex;
        }
        public async Task<int> ExecuteCommand(string commandText)
        {
            return await _appDbContext.Database.ExecuteSqlCommandAsync(commandText);
        }
        public DataTable GetDataFromRawSQL(string commandText)
        {
            string DevMode = WebConfigurationManager.AppSettings["DevMode"];
            string defaultCS = "DbCon";
            if(DevMode == "0")
            {
                defaultCS = "DbConNova";
            }

            string CS = ConfigurationManager.ConnectionStrings[defaultCS].ConnectionString;
            SqlConnection con = new SqlConnection(CS);

            SqlDataAdapter sda = new SqlDataAdapter(commandText,con);
            
            DataTable dt = new DataTable();
            sda.Fill(dt);

            return dt;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}