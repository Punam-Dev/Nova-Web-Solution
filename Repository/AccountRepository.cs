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
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext appDbContext;
        public AccountRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Users> GetUserByUserNameAndPassword(string userName, string password)
        {
            return await appDbContext.users.FirstOrDefaultAsync(user =>
                    user.UserName == userName && user.Password == password && user.Status == true
                );
        }

        public async Task<Users> CreateUser(Users users)
        {
            var result = appDbContext.users.Add(users);
            await appDbContext.SaveChangesAsync();
            return result;
        }

        public void DeleteUser(string userID)
        {
            var userForms = appDbContext.Forms.Where(x => x.FormsCreatedByUserID == userID).ToList();
            var userQuery = appDbContext.FormQuery.Where(x => x.FormQueryCreatedByUserID == userID).ToList();
            var userLogInDetails = appDbContext.UserLogInDetails.Where(x => x.UserID == userID).ToList();
            var user = appDbContext.users.Find(userID);

            if (user != null)
            {
                appDbContext.Forms.RemoveRange(userForms);
                appDbContext.FormQuery.RemoveRange(userQuery);
                appDbContext.UserLogInDetails.RemoveRange(userLogInDetails);
                appDbContext.users.Remove(user);

                appDbContext.SaveChanges();
            }
        }

        public Users GetUserByID(string userID)
        {
            return appDbContext.users.Find(userID);
        }

        public async Task<IEnumerable<UserListDto>> GetUsers()
        {
            var query = from user in appDbContext.users
                        join userForms in
                        (from f in appDbContext.Forms
                         where f.FormIsSubmit == true
                         group f by new { f.FormsCreatedByUserID } into r
                         select new
                         {
                             UserID = r.Key.FormsCreatedByUserID,
                             FormSubmited = r.Count()
                         }) on user.UserID equals userForms.UserID into userWithForms
                        from userForms in userWithForms.DefaultIfEmpty()
                        join userFormsQuery in
                        (from q in appDbContext.FormQuery
                         group q by new { q.FormQueryCreatedByUserID } into r
                         select new
                         {
                             UserID = r.Key.FormQueryCreatedByUserID,
                             QuerySubmited = r.Count()
                         })
                         on user.UserID equals userFormsQuery.UserID into userWithFormsAndQuery
                        from userFormsQuery in userWithFormsAndQuery.DefaultIfEmpty()
                        select new UserListDto
                        {
                            UserID = user.UserID,
                            UserName = user.UserName,
                            Password = user.Password,
                            UserRoles = user.UserRoles,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Phone = user.Phone,
                            Address = user.Address,
                            DOB = user.DOB,
                            CallerName = user.CallerName,
                            ActivationDate = user.ActivationDate,
                            Status = user.Status,
                            WorkStatus = user.WorkStatus,
                            OTP = user.OTP,
                            UserCreatedByUserID = user.UserCreatedByUserID,
                            UserCreatedDate = user.UserCreatedDate,
                            UserUpdatedByUserID = user.UserUpdatedByUserID,
                            UserUpdatedDate = user.UserUpdatedDate,
                            FormSubmited = userForms.FormSubmited,
                            QC = userFormsQuery.QuerySubmited
                        };

            List<UserListDto> lstUserListDto = await query.ToListAsync();
            lstUserListDto.ForEach(x => { if (x.FormSubmited == null) x.FormSubmited = 0; if (x.QC == null) x.QC = 0; });

            return lstUserListDto;
        }

        public async Task<Users> UpdateUser(Users users)
        {
            var user = await appDbContext.users.FindAsync(users.UserID);

            if (user != null)
            {
                user.UserName = users.UserName;
                user.Password = users.Password;
                user.FirstName = users.FirstName;
                user.LastName = users.LastName;
                user.Email = users.Email;
                user.Phone = users.Phone;
                user.Address = users.Address;
                user.DOB = users.DOB;
                user.CallerName = users.CallerName;
                user.Status = users.Status;
                user.WorkStatus = users.WorkStatus;

                appDbContext.Entry(user).State = EntityState.Modified;
                await appDbContext.SaveChangesAsync();
            }

            return null;
        }

        public async Task<Users> GetUserByUserName(string userName)
        {
            return await appDbContext.users.FirstOrDefaultAsync(user => user.UserName == userName);
        }

        public void UpdateWorkStatus(string userID, bool workStatus, string loggedInUserID)
        {

            var user = appDbContext.users.Find(userID);

            if (user != null && user.UserID != loggedInUserID)
            {
                user.WorkStatus = workStatus;

                appDbContext.Entry(user).State = EntityState.Modified;
                appDbContext.SaveChangesAsync();
            }
        }

        public async Task<int> StopWorkStatusOfAllUser(string excludeUserID)
        {
            //var users = await appDbContext.users.Where(x => x.UserID != excludeUserID).ToListAsync();

            //users.ForEach(x => x.WorkStatus = false);

            //appDbContext.users.Attach
            //appDbContext.users.UpdateRange(users);
            //return await appDbContext.SaveChangesAsync();

            return await appDbContext.Database.ExecuteSqlCommandAsync("Update Users SET WorkStatus = 0 Where UserID != {0}", excludeUserID);
        }
        public void SaveUserLogInDetails(UserLogInDetails userLogInDetails)
        {
            var result = appDbContext.UserLogInDetails.Add(userLogInDetails);
            appDbContext.SaveChanges();
        }

        public async Task<List<UserLogInDetails>> GetUserLogInDetailsByID(string userid)
        {
            return await appDbContext.UserLogInDetails.Where(x => x.UserID == userid).ToListAsync();
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