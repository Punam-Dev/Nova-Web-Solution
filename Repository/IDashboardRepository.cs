using NovaWebSolution.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaWebSolution.Repository
{
    public interface IDashboardRepository : IDisposable
    {
        Task<HomeCardDto> GetHomeCardDto(string userID);
    }
}
