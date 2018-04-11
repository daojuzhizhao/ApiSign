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
                name = "周游"
            };

            IHttpClientWrapper http = new HttpClientWapper(new  Md5Validator());
            var result = http.PostAsync<ApiResult>("http://localhost:911/", "api/default/gettest", dto, true);


            ViewBag.Message = JsonConvert.SerializeObject(result);

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