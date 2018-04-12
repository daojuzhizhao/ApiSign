using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 常量固定值
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// sign 效验码 key名称
        /// </summary>
        public const string HeaderSignKey = "sign";

        /// <summary>
        /// 密钥
        /// </summary>
        public static readonly string signKey = "000000";/// ConfigurationManager.AppSettings.Get("signKey");
    }
}
