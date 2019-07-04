using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkShop.Controllers
{
    public class BOOKController : Controller
    {
        //設定首頁
        Models.CodeService codeService = new Models.CodeService();
        public ActionResult Index()
        {
            ViewBag.KeeperData = this.codeService.GetKeeperSearch("", 0);
            ViewBag.BookClassData = this.codeService.GetClassTable("");
            ViewBag.BookStatusData = this.codeService.GetStatusTable("");
            return View();
        }
        [HttpPost()]
        public ActionResult Index(Models.BookSearch arg)
        {
            ViewBag.KeeperData = this.codeService.GetKeeperSearch("", 0);
            ViewBag.BookClassData = this.codeService.GetClassTable("");
            ViewBag.BookStatusData = this.codeService.GetStatusTable("");
            ViewBag.SearchResult = this.codeService.GetBookByCondtioin(arg);
            return View();
        }
        //新增功能的頁面
        [HttpGet()]
        public ActionResult InsertBook()
        {
            ViewBag.BookClassData = this.codeService.GetClassTable("");
            ViewBag.InsertResult = false;
            return View("InsertBook");
        }
        [HttpPost()]
        public ActionResult InsertBook(Models.Book arg)
        {
            ViewBag.BookClassData = this.codeService.GetClassTable("");
            codeService.InsertBookInfo(arg);
            return View("InsertBook");
        }
        //設定刪除功能
        [HttpPost()]
        public JsonResult DeleteBook(int BookID)
        {
            codeService.DeleteBook(BookID);
            return this.Json(true);
        }
    }

}