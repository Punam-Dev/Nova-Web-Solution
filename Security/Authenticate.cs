using NovaWebSolution.Models;
using NovaWebSolution.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NovaWebSolution.Security
{
    public class Authenticate: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string userId = Convert.ToString(context.HttpContext.Session["userid"]);

            if (string.IsNullOrEmpty(userId))
            {
                FormsAuthentication.SignOut();
                //context.HttpContext.Session.Clear();
                context.HttpContext.Session.Abandon(); // it will clear the session at the end of request
                context.HttpContext.Session.Clear();
                context.HttpContext.Session.RemoveAll();

                context.HttpContext.Response.Redirect("/Account/LogIn", true);
            }
            else
            {
                AccountRepository accountRepository = new AccountRepository(new AppDbContext());
                var user = accountRepository.GetUserByID(userId);
                if (user.WorkStatus == false)
                {
                    FormsAuthentication.SignOut();
                    context.HttpContext.Session.Abandon(); // it will clear the session at the end of request
                    context.HttpContext.Session.Clear();
                    context.HttpContext.Session.RemoveAll();

                    context.HttpContext.Response.Redirect("/Account/LogIn");
                }
            }
        }
    }
}