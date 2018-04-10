using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{

    public interface IHttpClientWrapper
    {

        /// <summary>
        /// 租户Id，即停车场Id
        /// </summary>
        string TenantId { get; set; }

        /// <summary>
        /// Http，不签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="baseUri"></param>
        /// <param name="relativeUrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<string> RequestAsync(HttpMethod method, string baseUri, string relativeUrl, object data);

        /// <summary>
        /// Http，仅返回ApiResult
        /// </summary>
        /// <param name="method">HttpMethod.Post, HttpMethod.Get</param>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">以json格式发送的数据</param>
        /// <param name="sign">是否签名</param>
        /// <param name="headers">附加的Headers</param>
        /// <param name="cancellationToken"></param>
        Task<T> RequestAsync<T>(HttpMethod method, string baseUri, string relativeUrl, object data,
            bool sign, Dictionary<string, string> headers, CancellationToken cancellationToken) where T : ApiResult;

        /// <summary>
        /// Get，仅返回ApiResult
        /// </summary>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">将转换为Url参数，并UrlEncode</param>
        /// <param name="sign">是否签名</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string baseUri, string relativeUrl, object data, bool sign) where T : ApiResult;

        /// <summary>
        /// Post，仅返回ApiResult
        /// </summary>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">以json格式发送的数据</param>
        /// <param name="sign">是否签名</param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string baseUri, string relativeUrl, object data, bool sign) where T : ApiResult;

        /// <summary>
        /// Post，仅返回ApiResult
        /// </summary>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">以json格式发送的数据</param>
        /// <param name="sign">是否签名</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string baseUri, string relativeUrl, object data, bool sign, CancellationToken cancellationToken) where T : ApiResult;

        /// <summary>
        /// Post，仅返回string
        /// </summary>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">以json格式发送的数据</param>
        /// <returns></returns>
        Task<string> PostAsync(string baseUri, string relativeUrl, object data);


        /// <summary>
        /// Get，不签名，返回任意类
        /// </summary>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">将转换为Url参数，并UrlEncode</param>
        /// <param name="sign">是否签名</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string baseUri, string relativeUrl, object data) where T : class;

        /// <summary>
        /// Post，不签名，返回任意类
        /// </summary>
        /// <param name="baseUri">请求接口的基地址，以http[s]://开头</param>
        /// <param name="relativeUrl">相对于基地址的接口地址，以/开头，不包含Url参数</param>
        /// <param name="data">以json格式发送的数据</param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string baseUri, string relativeUrl, object data) where T : class;
    }
}
