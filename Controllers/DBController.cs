using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NovaWebSolution.Dtos;
using NovaWebSolution.Models;
using NovaWebSolution.Repository;

namespace NovaWebSolution.Controllers
{
    public class DBController : Controller
    {
        private readonly IDBRepository dBRepository;
        public DBController()
        {
            dBRepository = new DBRepository(new AppDbContext());
        }
        [HttpGet]
        public ActionResult Index()
        {
            DBSqlDto dBSqlDto = new DBSqlDto();
            return View(dBSqlDto);
        }

        [HttpPost]
        public async Task<ActionResult> Index(DBSqlDto dBSqlDto, string submit)
        {
            if (!string.IsNullOrEmpty(submit))
            {
                if(submit.ToLower() == "execute")
                {
                    dBSqlDto.NoOfRowsAffected = await  dBRepository.ExecuteCommand(dBSqlDto.RawSQL);
                }
                else
                {
                    dBSqlDto.RawList = dBRepository.GetDataFromRawSQL(dBSqlDto.RawSQL);
                }
            }
            return View(dBSqlDto);
        }
    }
}