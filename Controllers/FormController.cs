using NovaWebSolution.Dtos;
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
using System.IO;
using System.Web.Hosting;

namespace NovaWebSolution.Controllers
{
    [Authorize]
    [Authenticate]
    public class FormController : Controller
    {
        private readonly IFormsRepository formsRepository;
        private readonly IAccountRepository accountRepository;
        private string LoggedInUserID;
        public FormController()
        {
            this.formsRepository = new FormsRepository(new AppDbContext());
            this.accountRepository = new AccountRepository(new AppDbContext());
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            LoggedInUserID = Convert.ToString(HttpContext.Session["userid"]);
            var user = accountRepository.GetUserByID(LoggedInUserID);

            if (user.WorkStatus == false || user.ActivationDate > DateTime.Now || user.IsActive == false )
            {
                return RedirectToAction("Report");
            }

            CreateFormDto createFormDto = new CreateFormDto();
            Forms forms = new Forms();

            //forms.FormNo = (await formsRepository.GetMaxFormNoOfUser(LoggedInUserID)) + 1;
            forms.FormNo = (await formsRepository.GetTotalSubmitedFormOfUser(LoggedInUserID)) + 1;
            forms.FormImagePath = "~/QuestionsImages/" + forms.FormNo.ToString() + ".PNG";

            createFormDto.Forms = forms;

            createFormDto.MaxFormCountOfUser = await formsRepository.GetMaxFormNoOfUser(LoggedInUserID);

            return View(createFormDto);
        }

        [Route("Form/Create/{id}")]
        [HttpGet]
        public async Task<ActionResult> Create(string id)
        {
            LoggedInUserID = Convert.ToString(HttpContext.Session["userid"]);
            var user = accountRepository.GetUserByID(LoggedInUserID);

            if (user.WorkStatus == false || user.ActivationDate > DateTime.Now || user.IsActive == false)
            {
                return RedirectToAction("Report");
            }

            if (!string.IsNullOrEmpty(id))
            {
                var Form = await formsRepository.GetFormByID(Convert.ToInt64(id));

                CreateFormDto createFormDto = new CreateFormDto();

                Forms forms = new Forms();

                forms.FormsID = Form.FormsID;
                forms.FirstName = Form.FirstName;
                forms.LastName = Form.LastName;
                forms.Email = Form.Email;
                forms.SSN = Form.SSN;
                forms.Phone = Form.Phone;
                forms.BankName = Form.BankName;
                forms.AccountNo = Form.AccountNo;
                forms.LoanAmount = Form.LoanAmount;
                forms.Address = Form.Address;
                forms.City = Form.City;
                forms.State = Form.State;
                forms.Zip = Form.Zip;
                forms.DOB = Form.DOB;
                forms.LicenceNo = Form.LicenceNo;
                forms.LicenceState = Form.LicenceState;
                forms.IP = Form.IP;
                forms.FormNo = Form.FormNo;
                forms.FormImagePath = Form.FormImagePath;

                createFormDto.Forms = forms;

                List<FormQueryDto> formQueryDtos = new List<FormQueryDto>()
                {
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "First Name" } , IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Last Name" } , IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Email" } , IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "SSN" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Phone" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Bank Name" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "A/C No" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() {FormQueryText = "Loan Amount" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "City" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "State" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "ZIP" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Date of Birth" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Licence No." }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "Licence State" }, IsChecked = false },
                    new FormQueryDto {  FormQuery = new FormQuery() { FormQueryText = "IP" }, IsChecked = false },
                };

                createFormDto.formQueryDtos = formQueryDtos;

                return View(createFormDto);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateFormDto createFormDto, string submit, string id)
        {
            Forms forms = new Forms();

            forms.FirstName = createFormDto.Forms.FirstName;
            forms.LastName = createFormDto.Forms.LastName;
            forms.Email = createFormDto.Forms.Email;
            forms.SSN = createFormDto.Forms.SSN;
            forms.Phone = createFormDto.Forms.Phone;
            forms.BankName = createFormDto.Forms.BankName;
            forms.AccountNo = createFormDto.Forms.AccountNo;
            forms.LoanAmount = createFormDto.Forms.LoanAmount;
            forms.Address = createFormDto.Forms.Address;
            forms.City = createFormDto.Forms.City;
            forms.State = createFormDto.Forms.State;
            forms.Zip = createFormDto.Forms.Zip;
            forms.DOB = createFormDto.Forms.DOB;
            forms.LicenceNo = createFormDto.Forms.LicenceNo;
            forms.LicenceState = createFormDto.Forms.LicenceState;
            forms.IP = createFormDto.Forms.IP;
            forms.FormNo = createFormDto.Forms.FormNo;
            forms.FormImagePath = createFormDto.Forms.FormImagePath;

            if (submit == "Submit")
            {
                forms.FormIsSubmit = true;
            }
            else
            {
                forms.FormIsSubmit = false;
            }
            forms.FormsCreatedDate = DateTime.Now;

            if (TempData.ContainsKey("UserID"))
            {
                LoggedInUserID = TempData.Peek("UserID").ToString();
            }

            if (!string.IsNullOrEmpty(LoggedInUserID))
            {
                forms.FormsCreatedByUserID = LoggedInUserID;
            }
            else
            {
                return RedirectToAction("LogIn", "Account");
            }

            var createdForm = await formsRepository.CreateForm(forms);

            if (forms.FormIsSubmit)
            {
                ToastrNotificationService.AddSuccessNotification("Form Submited Successfully", null);
                return RedirectToAction("Index", new { id = "submit" });
            }

            ToastrNotificationService.AddSuccessNotification("Form Saved Successfully", null);

            return RedirectToAction("Index", new { id = "save" });
        }

        [Route("Form/Index/{id}")]
        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            LoggedInUserID = Convert.ToString(HttpContext.Session["userid"]);
            var user = accountRepository.GetUserByID(LoggedInUserID);

            if (user.WorkStatus == false || user.ActivationDate > DateTime.Now || user.IsActive == false)
            {
                return RedirectToAction("Report");
            }

            bool? isSubmit;
            if (id.ToLower() == "save")
            {
                isSubmit = false;
            }
            else if (id.ToLower() == "submit")
            {
                isSubmit = true;
            }
            else
            {
                isSubmit = null;
            }

            if (TempData.ContainsKey("UserID"))
            {
                LoggedInUserID = TempData.Peek("UserID").ToString();
            }

            var forms = await formsRepository.GetForms(isSubmit, LoggedInUserID);
            return View(forms);
        }

        [HttpPost]
        public ActionResult FormQuery(CreateFormDto createFormDto, string id)
        {
            List<FormQuery> formQueryList = new List<FormQuery>();

            foreach (var item in createFormDto.formQueryDtos)
            {
                if (item.IsChecked)
                {
                    FormQuery formQuery = new FormQuery();

                    formQuery.FormQueryText = Convert.ToString(item.FormQuery.FormQueryText);
                    formQuery.FormQueryStatus = "Pending";
                    formQuery.FormQueryCreatedDate = DateTime.Now;

                    if (TempData.ContainsKey("UserID"))
                    {
                        LoggedInUserID = TempData.Peek("UserID").ToString();
                    }

                    if (!string.IsNullOrEmpty(LoggedInUserID))
                    {
                        formQuery.FormQueryCreatedByUserID = LoggedInUserID;
                    }
                    else
                    {
                        return RedirectToAction("LogIn", "Account");
                    }

                    formQueryList.Add(formQuery);
                }
            }

            formsRepository.CreateFormQuery(formQueryList);

            ToastrNotificationService.AddSuccessNotification("Query Saved Successfully", null);

            return RedirectToAction("Create", "Form", new { id = id });
        }

        [HttpGet]
        public async Task<ActionResult> Query()
        {
            LoggedInUserID = Convert.ToString(HttpContext.Session["userid"]);
            var user = accountRepository.GetUserByID(LoggedInUserID);

            if (user.WorkStatus == false || user.ActivationDate > DateTime.Now || user.IsActive == false)
            {
                return RedirectToAction("Report");
            }

            if (TempData.ContainsKey("UserID"))
            {
                LoggedInUserID = TempData.Peek("UserID").ToString();
            }

            if (!string.IsNullOrEmpty(LoggedInUserID))
            {
                var formQuery = await formsRepository.GetFormQueries(LoggedInUserID);
                return View(formQuery);
            }
            return RedirectToAction("LogIn", "Account");
        }

        [HttpGet]
        public ActionResult Report()
        {
            return View();
        }
    }
}