using NovaWebSolution.Dtos;
using NovaWebSolution.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NovaWebSolution.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext appDbContext;
        public DashboardRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<HomeCardDto> GetHomeCardDto(string userID)
        {
            var result = from form in appDbContext.Forms
                         where form.FormsCreatedByUserID == userID
                         group form by new { form.FormsCreatedByUserID } into x
                         select new HomeCardDto
                         {
                             TotalForm = x.Count(),
                             SaveForm = x.Where(m => m.FormIsSubmit == false).Count(),
                             SubmitForm = x.Where(m => m.FormIsSubmit == true).Count(),
                         };

            return await result.FirstOrDefaultAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    appDbContext.Dispose();
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