using NovaWebSolution.Models;
using NovaWebSolution.Repository;
using NovaWebSolution.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NovaWebSolution.Services;

namespace NovaWebSolution.Controllers
{
    [Authorize]
    [Authenticate]
    public class UsersController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly AuthMailSender _emailSender;
        public UsersController()
        {
            this.accountRepository = new AccountRepository(new AppDbContext());
            _emailSender = new AuthMailSender();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var users = await accountRepository.GetUsers();

            return View(users);
        }

        [HttpPost]
        public async Task<ActionResult> StopWorkStatus()
        {
            string userID = Convert.ToString(Session["userid"]);
            var noOfRowsAffected = await accountRepository.StopWorkStatusOfAllUser(userID);

            ToastrNotificationService.AddSuccessNotification("Work Status of all user are changed to inactive", null);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            string loggedInUserID = Convert.ToString(Session["userid"]);

            if (id == loggedInUserID)
            {
                ToastrNotificationService.AddWarningNotification("You can not delete yourself", null);
            }
            else
            {
                accountRepository.DeleteUser(id);
                ToastrNotificationService.AddSuccessNotification("User deleted successfully", null);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ChangeWorkStatus(string userID, bool workStatus)
        {
            string loggedInUserID = Convert.ToString(Session["userid"]);
            accountRepository.UpdateWorkStatus(userID, workStatus, loggedInUserID);

            string strWorkStatus = workStatus == true ? "active" : "inactive";
            if (userID == loggedInUserID)
            {
                ToastrNotificationService.AddWarningNotification("You can not change your Work Status yourself", null);
            }
            else
            {
                ToastrNotificationService.AddSuccessNotification("Work Status changed succesfully to " + strWorkStatus, null);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Users users)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(users.UserID))
                {
                    users.UserID = Guid.NewGuid().ToString();
                    users.UserCreatedDate = DateTime.Now;
                    users.UserCreatedByUserID = Convert.ToString( HttpContext.Session["userid"]);
                    users.UserRoles = "user";

                    Random rnd = new Random();
                    int randomNumber = rnd.Next(100000, 999999);

                    users.OTP = randomNumber;
                    await accountRepository.CreateUser(users);

                    string verifyOTPUrl = $"{this.Request.Url.AbsoluteUri}Account/VerifyOTP/{users.UserID}";

                    string body = "Hello " + users.FirstName + " " + users.LastName + "," +
                        "<br/><br/> Welcome to <b>Nova Web Solution</b>" +
                        "<br/><br/> Please click the following link to activate your account " +
                        "<br/><a href=\"" + verifyOTPUrl + "\">click here</a><br/>" +
                        "Use OTP code " + randomNumber.ToString() + " to activate your account.<br/><br/>" +
                        "URL to Login:<a href=\"https://novawebsolution.co.in\">https://novawebsolution.co.in</a> <br/>Thank You.";
                    await _emailSender.SendEmailAsync(users.Email, "Welcome to Nova Web Solution", body);

                    ToastrNotificationService.AddSuccessNotification("User Created Succesfully", null);

                    return RedirectToAction("Index");
                }
            }
            return View(users);
        }
    }
}