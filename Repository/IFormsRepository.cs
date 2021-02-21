using NovaWebSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaWebSolution.Repository
{
    interface IFormsRepository : IDisposable
    {
        Task<Forms> GetFormByID(Int64 FormID);
        Task<IEnumerable<Forms>> GetForms(bool? isSubmit, string LoggedInUserID);
        Task<Forms> CreateForm(Forms Forms);
        Task<Forms> UpdateForm(Forms Forms);
        void DeleteForm(Int64 FormID);
        Task<IEnumerable<FormQuery>> GetFormQueries(string LoggedInUserID);
        void CreateFormQuery(List<FormQuery> formQueries);
    }
}
