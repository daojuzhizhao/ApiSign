using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IHashValidator
    {
        /// <summary>
        /// 获取类似QueryString的键值对
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GetKeyValuePairString(object data);

        /// <summary>
        /// 计算Hash
        /// </summary>
        /// <returns></returns>
        string ComputeHashFromString(string raw);

        /// <summary>
        /// 计算Hash
        /// </summary>
        /// <returns></returns>
        string ComputeHashFromObject(object data);

        /// <summary>
        /// 验证给定的字符串和校验码是否一致
        /// </summary>
        /// <param name="sign">校验码</param>
        /// <param name="raw">需要验证的字符串</param>
        /// <returns></returns>
        bool Validate(string sign, string raw);

        /// <summary>
        /// 验证给定的字符串和校验码是否一致
        /// </summary>
        /// <param name="sign">校验码</param>
        /// <param name="data">需要验证的对象</param>
        /// <returns></returns>
        bool ValidateObject(string sign, object data);
    }
}
