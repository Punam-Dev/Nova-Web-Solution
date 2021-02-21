using NovaWebSolution.Dtos;
using NovaWebSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaWebSolution.Repository
{
    public interface IAccountRepository : IDisposable
    {
        Users GetUserByID(string userID);
        Task<IEnumerable<UserListDto>> GetUsers();
        Task<Users> CreateUser(Users users);
        Task<Users> UpdateUser(Users users);
        void DeleteUser(string userID);
        Task<Users> GetUserByUserNameAndPassword(string userName, string password);
        Task<Users> GetUserByUserName(string userName);
        void UpdateWorkStatus(string userID, bool workStatus, string loggedInUserID);
        Task<int> StopWorkStatusOfAllUser(string excludeUserID);
    }
}
