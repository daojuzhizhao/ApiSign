using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.API
{
    public class Default1Controller : ApiController
    {
        [HttpPost]
        public ApiResult GetTest([FromBody]TestDto dto)
        {
            return new ApiResult();
        }

        [HttpGet]
        public ApiResult test1()
        {

            return new ApiResult();
        }


        [HttpPost]
        public ApiResult GetTest1(TestDto dto)
        {
            return new ApiResult();
        }
    }
}
