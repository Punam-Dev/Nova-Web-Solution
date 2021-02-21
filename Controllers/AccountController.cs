using NovaWebSolution.Dtos;
using NovaWebSolution.Models;
using NovaWebSolution.Repository;
using NovaWebSolution.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NovaWebSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly AuthMailSender _emailSender;
        public AccountController()
        {
            this.accountRepository = new AccountRepository(new AppDbContext());
            _emailSender = new AuthMailSender();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(UserLogInDto userLogInDto, string returnUrl)
        {
            var user = await accountRepository.GetUserByUserNameAndPassword(userLogInDto.UserName, userLogInDto.Password);
            UserLogInDetails userLogInDetails = new UserLogInDetails();

            if (user != null)
            {
                //var claims = new List<Claim>();

                //claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                //string[] roles = user.UserRoles.Split(',');

                //foreach (string role in roles)
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, role));
                //}

                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //var principal = new ClaimsPrincipal(identity);

                //var props = new AuthenticationProperties();
                //props.IsPersistent = false;

                //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                FormsAuthentication.SetAuthCookie(userLogInDto.UserName, false);

                if (Request.QueryString["ReturnUrl"] != null)
                {
                    return Redirect(Request.QueryString["ReturnUrl"].ToString());
                }


                userLogInDetails.UserIP = Request.UserHostAddress.ToString();

                TempData["UserID"] = user.UserID;

                HttpContext.Session["userid"] =  user.UserID;
                HttpContext.Session["username"]= user.UserName;
                HttpContext.Session["roles"]= user.UserRoles;

                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("error", "Invalid username or password");
            return View();
        }

        [HttpGet]
        [Route("Account/SignUp/{id}")]
        public ActionResult SignUp(string id)
        {
            // fetch user by id. that no, name and email will go in agreement
            var user = accountRepository.GetUserByID(id);
            SignUpDto signUpDto = new SignUpDto();
            if (user != null)
            {
                signUpDto.FirstName = user.FirstName;
                signUpDto.LastName = user.LastName;
                signUpDto.Email = user.Email;
                signUpDto.Phone = user.Phone;
                signUpDto.CallerName = user.CallerName;
                if (!string.IsNullOrEmpty(Convert.ToString(user.OTP)) || user.OTP != 0)
                {
                    if (user.ActivationDate != null || !string.IsNullOrEmpty(Convert.ToString(user.ActivationDate)))
                    {
                        return RedirectToAction("LogIn");
                    }
                }
                else
                {
                    return RedirectToAction("VerifyOTP", new { id = id });
                }
            }
            ModelState.AddModelError("error", "Invalid user");
            return View(signUpDto);
        }

        [HttpPost]
        [Route("Account/SignUp/{id}")]
        public async Task<JsonResult> SignUp(string id, SignUpDto signUpDto)
        {
            try
            {
                var user = accountRepository.GetUserByID(id);
                if (user != null)
                {
                    String path = "~/DigitalSign";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    signUpDto.DigitalSignatureData = signUpDto.DigitalSignatureData.Replace("data:image/jpeg;base64,", "");
                    path = path + "//Sign_" + id + ".jpg";
                    user.DigitalSignPath = path;

                    byte[] bytes = Convert.FromBase64String(signUpDto.DigitalSignatureData);

                    System.IO.File.WriteAllBytes(path, bytes);


                    Random random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm0123456789";
                    int length = random.Next(6, 10);

                    string userName = new string(Enumerable.Repeat(chars, length)
                      .Select(s => s[random.Next(s.Length)]).ToArray());

                    length = random.Next(4, 6);
                    string password = new string(Enumerable.Repeat(chars, length)
                      .Select(s => s[random.Next(s.Length)]).ToArray());

                    user.ActivationDate = signUpDto.ActivationDate.Value.AddDays(1);
                    user.OTP = null;
                    user.WorkStatus = true;
                    user.Status = true;
                    user.UserName = userName;
                    user.Password = password;
                    await accountRepository.UpdateUser(user);

                    string body = "Hello " + user.FirstName + " " + user.LastName + "," +
                        "<br/><br/> Welcome to dotindiapvtltd" +
                        "<br/><br/> Your login credential is, " +
                        "<br/>Username: " + userName +
                        "<br/>Password: " + password +
                        "<br/><br/>URL to Login:<a href=\"https://dotindiapvtltd.in\">https://dotindiapvtltd.in</a> <br/>Thank You.";
                    await _emailSender.SendEmailAsync(user.Email, "Welcome to dotindiapvtltd", body);

                    return Json(signUpDto);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            // fetch user by id. that no, name and email will go in agreement

            ModelState.AddModelError("error", "Invalid user");
            return Json(signUpDto);
        }

        [HttpGet]
        [Route("Account/VerifyOTP/{id}")]
        public ActionResult VerifyOTP(string id)
        {
            // fetch user by id. that no, name and email will go in agreement
            var user = accountRepository.GetUserByID(id);
            if (user != null)
            {
                if (string.IsNullOrEmpty(Convert.ToString(user.OTP)) || user.OTP == 0)
                {
                    if (user.ActivationDate == null || string.IsNullOrEmpty(Convert.ToString(user.ActivationDate)))
                    {
                        return RedirectToAction("SignUp", new { id = id });
                    }
                    return RedirectToAction("LogIn");
                }
                return View();
            }
            ModelState.AddModelError("error", "Invalid user");
            return View();
        }

        [HttpPost]
        [Route("Account/VerifyOTP/{id}")]
        public async Task<ActionResult> VerifyOTP(VerifyOTPDto verifyOTPDto, string id)
        {
            var user = accountRepository.GetUserByID(id);
            if (user != null)
            {
                if (user.OTP == verifyOTPDto.OTP)
                {
                    user.OTP = null;
                    await accountRepository.UpdateUser(user);

                    return RedirectToAction("SignUp", new { id = id });
                }
                return View(verifyOTPDto);
            }
            ModelState.AddModelError("error", "Invalid user");
            return View();
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon(); // it will clear the session at the end of request
                Session.Clear();
                Session.RemoveAll();
                HttpContext.Session.Clear();
            }
            return RedirectToAction("LogIn");
        }
    }
}