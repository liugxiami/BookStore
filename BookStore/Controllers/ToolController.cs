using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class ToolController : Controller
    {
        // GET: Tool
        [HttpGet]
        public ActionResult Calculate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Calculate(double x, double y, string opt)
        {
            double result = 0;
            if (!string.IsNullOrEmpty(opt))
            {
                if (opt == "add")
                {
                    result = x + y;
                }
                else if (opt == "sub")
                {
                    result = x - y;
                }
                else if (opt == "mul")
                {
                    result = x * y;
                }
                else if (opt == "div")
                {
                    result = x / y;
                }
                else
                {
                    throw new Exception("Invalid input");
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}