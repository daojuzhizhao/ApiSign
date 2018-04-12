using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TestDto dto = new TestDto()
            {
                address = "天府软件园E6-1115",
                age = 28,
                name = "周游",
                data = new Test2[]
                {
                    new Test2{phone="15228834746"},
                    new Test2{phone="11111111111"},
                    new Test2{phone="22222222222"},
                    new Test2{phone="33333333333"},

                }
            };

            List<Test2> list = new List<Test2>{
                    new Test2{phone="15228834746"},
                    new Test2{phone="11111111111"},
                    new Test2{phone="22222222222"},
                    new Test2{phone="33333333333"},
            };

            string sign = new Md5Validator().ComputeHashFromObject(dto);

            ViewBag.sign = sign;
           ViewBag.Message = JsonConvert.SerializeObject(list);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}