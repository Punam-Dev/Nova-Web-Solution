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
using Microsoft.Reporting.WebForms;
using NovaWebSolution.Dtos;
using System.Web.Configuration;

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
            var roles = HttpContext.Session["roles"];
            if(roles != null)
            {
                if(roles.ToString().ToLower() != "admin")
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            string LoggedInUserID = Convert.ToString(HttpContext.Session["userid"]);
            var user = accountRepository.GetUserByID(LoggedInUserID);

            if (user.WorkStatus == false || user.ActivationDate > DateTime.Now || user.IsActive == false)
            {
                return RedirectToAction("Report");
            }

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
        public ActionResult ChangeWorkStatus(string userID, bool workStatus)
        {
            string loggedInUserID = Convert.ToString(Session["userid"]);
            string strWorkStatus = workStatus == true ? "active" : "inactive";

            if (userID == loggedInUserID)
            {
                return Json(new { success = false, responseText = "You can not change your Work Status yourself" }, JsonRequestBehavior.AllowGet);
            }

            accountRepository.UpdateWorkStatus(userID, workStatus, loggedInUserID);

            return Json(new { success = true, responseText = $"Work Status changed succesfully to { strWorkStatus }" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            string LoggedInUserID = Convert.ToString(HttpContext.Session["userid"]);
            var user = accountRepository.GetUserByID(LoggedInUserID);

            if (user.WorkStatus == false || user.ActivationDate > DateTime.Now || user.IsActive == false)
            {
                return RedirectToAction("Report");
            }

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
                    users.UserCreatedByUserID = Convert.ToString(HttpContext.Session["userid"]);
                    users.IsActive = true;
                    users.MaxFormsCount = 698;
                    users.UserRoles = "user";

                    Random rnd = new Random();
                    int randomNumber = rnd.Next(100000, 999999);

                    users.OTP = randomNumber;
                    await accountRepository.CreateUser(users);

                    string strURL = this.Request.Url.Scheme + "://" + this.Request.Url.Host;

                    if (!string.IsNullOrEmpty(Convert.ToString(this.Request.Url.Port)))
                    {
                        strURL = strURL + ":" + Convert.ToString(this.Request.Url.Port);
                    }

                    string verifyOTPUrl = $"{strURL}/Account/VerifyOTP/{users.UserID}";

                    string companyName = WebConfigurationManager.AppSettings["CompanyName"];
                    string companyURL = WebConfigurationManager.AppSettings["CompanyURL"];

                    string body = "Hello " + users.FirstName + " " + users.LastName + "," +
                        "<br/><br/> Welcome to <b>" + companyName  + "</b>" +
                        "<br/><br/> Please click the following link to activate your account " +
                        "<br/><a href=\"" + verifyOTPUrl + "\">click here</a><br/>" +
                        "Use OTP code " + randomNumber.ToString() + " to activate your account.<br/><br/>" +
                        "URL to Login:<a href=\"" + companyURL + "\">" + companyURL + "</a> <br/>Thank You.";
                    await _emailSender.SendEmailAsync(users.Email, "Welcome to " + companyName, body);

                    ToastrNotificationService.AddSuccessNotification("User Created Succesfully", null);

                    return RedirectToAction("Index");
                }
            }
            return View(users);
        }

        [HttpPost]
        public async Task<ActionResult> UserLogInDetails(string userid)
        {
            var userLogInDetails = await accountRepository.GetUserLogInDetailsByID(userid);
            return PartialView("UserLogInDetails", userLogInDetails);
        }

        [HttpPost]
        public async Task<FileResult> DownloadReport(string id, string firstName, string lastName)
        {
            var result = await accountRepository.GetUserLogInDetailsByID(id);

            //string RptPath = Server.MapPath("~/RDLC/rptUserLogInDetails.rdlc");
            string RptPath = Server.MapPath("~/RDLC/rptUserLogInDetails.rdlc");

            ReportViewer rv = new ReportViewer();

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = result;

            string companyName = WebConfigurationManager.AppSettings["CompanyName"];

            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("username",  firstName + " " + lastName);
            parameters[1] = new ReportParameter("companyName", companyName);

            //ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = RptPath;

            // Add the new report datasource to the report.
            rv.LocalReport.DataSources.Add(rds);
            rv.LocalReport.EnableHyperlinks = true;

            rv.LocalReport.SetParameters(parameters);

            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            string fileName = "LogInDetails-" + firstName + lastName + ".pdf";
            return File(streamBytes, mimeType, fileName);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (ModelState.IsValid)
            {
                string userid = Convert.ToString(HttpContext.Session["userid"]);

                if (!string.IsNullOrEmpty(userid))
                {
                    if(changePasswordDto.Password == changePasswordDto.NewPassword)
                    {
                        ToastrNotificationService.AddWarningNotification("Password and New Password can't be same", null);
                    }
                    else if(changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                    {
                        ToastrNotificationService.AddWarningNotification("New Password and Confirm Password must be same", null);
                    }
                    else
                    {
                        var user = accountRepository.GetUserByID(userid);
                        if(user != null)
                        {
                            if(user.Password == changePasswordDto.Password)
                            {
                                await accountRepository.ChangePassword(changePasswordDto);
                                ToastrNotificationService.AddSuccessNotification("Password changed successfully", null);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ToastrNotificationService.AddWarningNotification("Invaild Password", null);
                                ModelState.AddModelError("Password", "Invaild Password");
                            }
                        }
                        
                    }
                }
            }
            
            return View(changePasswordDto);
        }
    }
}