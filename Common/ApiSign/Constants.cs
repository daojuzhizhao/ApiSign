using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Constants
    {
        public const string HeaderSignKey = "sign";

        public static readonly string signKey = ConfigurationManager.AppSettings.Get("signKey");
    }
}
