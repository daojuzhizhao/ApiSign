﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.API
{

    [SignValidate]
    public class DefaultController : ApiController
    {
        [HttpPost]
        public ApiResult<TestDto> GetTest([FromBody]TestDto dto)
        {
            return new ApiResult<TestDto>(dto);
        }
    }
}
