using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaWebSolution.Repository
{
    public interface IDBRepository: IDisposable
    {
        Task<int> ExecuteCommand(string commandText);
        DataTable GetDataFromRawSQL(string commandText);
    }
}
