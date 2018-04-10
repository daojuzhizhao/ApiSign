using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// DTO基类，为了便于签名，约定：DTO的属性类型只能是值类型、string或者DtoBase的子类
    /// </summary>
    public abstract class DtoBase
    {
        public override string ToString()
        {
            return KeyValueConverter.ToKeyValuePairString(this);
            //https://www.cnblogs.com/shihaiming/p/6227542.html
        }
    }
}
