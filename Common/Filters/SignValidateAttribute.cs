using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common
{

    //[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SignValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            string msg = string.Empty;

            var signDic = context.Request.Headers.Where(h => h.Key == Constants.HeaderSignKey).FirstOrDefault();
            var sign = signDic.Key != null ? signDic.Value.FirstOrDefault() : string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                var validator = new Md5Validator();
                var raw = string.Empty;

                if (context.Request.Method == HttpMethod.Get) // from uri
                {
                    if (context.Request.GetQueryNameValuePairs().Count() > 0)
                    {
                        raw = string.Join("&", context.Request.GetQueryNameValuePairs().OrderBy(x => x.Key).Select(a => a.Key + "=" + a.Value).ToArray());
                        if (validator.Validate(sign, raw))
                        {
                            return;
                        }
                    }
                    else
                    {
                        // 没有参数
                        return;
                    }
                }
                else // post from body, and olny one argument (DTO)
                {
                    // post 必须有参数
                    if (context.ActionArguments.Values.Count > 0)
                    {
                        if (validator.ValidateObject(sign, context.ActionArguments.Values.First()))
                        {
                            return;
                        }
                    }
                }
#if DEBUG
                msg = "参数不合法";
#endif

            }
            else
            {
#if DEBUG
                msg = "缺少校验码";
#endif
            }

            context.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(msg, Encoding.GetEncoding("UTF-8"))

            };
            base.OnActionExecuting(context);


        }
        
    }

}
