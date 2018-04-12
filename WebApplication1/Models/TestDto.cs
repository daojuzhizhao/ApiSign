using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class TestDto:DtoBase
    {
        public string name { get; set; }

        public int age { get; set; }

        public string address { get; set; }


        public Test2[] data { get; set; }
    }

    public class Test2
    {
        public string phone { get; set; }
    }
}