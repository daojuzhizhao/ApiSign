using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SignValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sign = context.HttpContext.Request.Headers["sign"];
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
                    context.HttpContext.Response.StatusCode = 500;
                    context.Result = new JsonResult(new
                    {
                        Code = -500,
                        Message = errorString
                    });
                    return;
                }
                var validator = context.HttpContext.RequestServices.GetService<IHashValidator>();
                var raw = string.Empty;

                if (context.HttpContext.Request.Method.ToLower() == "get") // from uri
                {
                    raw = string.Join('&', context.HttpContext.Request.Query.OrderBy(x => x.Key).Select(a => a.Key + '=' + a.Value).ToArray());
                }
                else // post from body, and olny one argument (DTO)
                {
                    if (context.ActionArguments.Values.FirstOrDefault() is DtoBase dto)
                    {
                        raw = dto.ToString();
                    }
                }

                if (validator.Validate(sign, raw))
                {
                    return;
                }
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
