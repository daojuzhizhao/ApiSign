using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Common
{

    //[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SignValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {

            var signDic = context.Request.Headers.Where(h => h.Key == Constants.HeaderSignKey).FirstOrDefault();
            var sign = signDic.Key != null ? signDic.Value.FirstOrDefault() : string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                if (!context.ModelState.IsValid)
                {
                    // 获取所有错误的Key 
                    var keys = context.ModelState.Keys.ToList();
                    // 获取每一个key对应的ModelStateDictionary 
                    var errorString = string.Empty;
                    errorString = keys.Select(key => context.ModelState[key].Errors.ToList()).SelectMany(errors => errors).Aggregate(errorString, (current, error) => current + (error.ErrorMessage + ";"));
                    if (errorString.StartsWith(";")) errorString = errorString.Remove(0, 1);
                    if (string.IsNullOrEmpty(errorString))
                    {
                        errorString = "参数不合法";
                    }

                    context.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ReasonPhrase = errorString
                    };
                    return;
                }
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
            }


            context.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            };
        }
    }
}
