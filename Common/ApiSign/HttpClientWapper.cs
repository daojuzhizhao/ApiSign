using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class HttpClientWapper : IHttpClientWrapper, IDisposable
    {
        private const string DefaultAccept = "application/json";
        private readonly IHashValidator _validator;
        private readonly HttpClient _httpClient;

        public HttpClientWapper(IHashValidator validator)
        {
            _validator = validator;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(DefaultAccept));
            _httpClient.DefaultRequestHeaders.Add("KeepAlive", "true");
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            this.TenantId = "0";
        }

        #region IHttpClientWrapper

        private string _tenantId;

        /// <inheritdoc />
        public string TenantId
        {
            get => _tenantId;
            set
            {
                _tenantId = value;
                _httpClient.DefaultRequestHeaders.Remove("tenant");
                _httpClient.DefaultRequestHeaders.Add("tenant", _tenantId);
            }
        }

        public async Task<string> RequestAsync(HttpMethod method, string baseUri, string relativeUrl, object data, bool sign, Dictionary<string, string> headers, CancellationToken token)
        {
            // prepare request message
            var request = new HttpRequestMessage();
            request.Method = method;

            if (sign) // 添加签名
            {
                request.Headers.Add(Constants.HeaderSignKey, _validator.ComputeHashFromObject(data));
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (item.Key != Constants.HeaderSignKey)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
            }


            if (method == HttpMethod.Get)
            {
                var queryString = _validator.GetKeyValuePairString(data);
                if (!string.IsNullOrEmpty(queryString))
                {
                    // relativeUrl += "?" + WebUtility.UrlEncode(queryString);
                    relativeUrl += "?" + queryString;
                }
            }
            else if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            }
            else
            {
                throw new ArgumentException($"不支持{method}");
            }
            request.RequestUri = new Uri(new Uri(baseUri), relativeUrl);

            // get response
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, token);

            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"请求外部服务器失败，{ex.InnerException?.Message ?? ex.Message}，Url为{baseUri + relativeUrl}");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"请求外部服务器失败，Status Code为{response.StatusCode}，Url为{baseUri + relativeUrl}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <inheritdoc />
        public async Task<T> RequestAsync<T>(HttpMethod method, string baseUri, string relativeUrl, object data, bool sign, Dictionary<string, string> headers, CancellationToken cancellationToken) where T : ApiResult
        {
            // prepare request message
            var request = new HttpRequestMessage();
            request.Method = method;
            if (sign) // 添加签名
            {
                request.Headers.Add(Constants.HeaderSignKey, _validator.ComputeHashFromObject(data));
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (item.Key != Constants.HeaderSignKey)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
            }

            if (method == HttpMethod.Get)
            {
                var queryString = _validator.GetKeyValuePairString(data);
                if (!string.IsNullOrEmpty(queryString))
                {
                    // relativeUrl += "?" + WebUtility.UrlEncode(queryString);
                    relativeUrl += "?" + queryString;
                }
            }
            else if (method == HttpMethod.Post)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, DefaultAccept);
            }
            else
            {
                throw new ArgumentException("仅支持Get和Post");
            }
            request.RequestUri = new Uri(new Uri(baseUri), relativeUrl);

            // get response
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"请求外部服务器失败，Status Code为{response.StatusCode}，Url为{baseUri + relativeUrl}");
            }
            var responseText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(responseText);
            if (sign) // 验证签名
            {
                var responseSign = response.Headers.GetValues(Constants.HeaderSignKey).FirstOrDefault();
                if (string.IsNullOrEmpty(responseSign))
                {
                    throw new HttpRequestException($"请求外部服务器失败，未返回签名，Status Code为{response.StatusCode}，Url为{baseUri + relativeUrl}");
                }

                if (!_validator.ValidateObject(responseSign, result))
                {
                    throw new HttpRequestException($"请求外部服务器失败，验证签名失败，签名值为{responseSign}，Status Code为{response.StatusCode}，Url为{baseUri + relativeUrl}");
                }
            }
            return result;
        }

        /// <inheritdoc />
        public Task<string> RequestAsync(HttpMethod method, string baseUri, string relativeUrl, object data)
        {
            return RequestAsync(method, baseUri, relativeUrl, data, false, null, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task<T> GetAsync<T>(string baseUri, string relativeUrl, object data, bool sign) where T : ApiResult
        {
            return RequestAsync<T>(HttpMethod.Get, baseUri, relativeUrl, data, sign,null, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task<T> PostAsync<T>(string baseUri, string relativeUrl, object data, bool sign) where T : ApiResult
        {
            return RequestAsync<T>(HttpMethod.Post, baseUri, relativeUrl, data, sign,null, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task<T> PostAsync<T>(string baseUri, string relativeUrl, object data, bool sign,
            CancellationToken cancellationToken) where T : ApiResult
        {
            return RequestAsync<T>(HttpMethod.Post, baseUri, relativeUrl, data, sign, null, cancellationToken);
        }

        /// <inheritdoc />
        public Task<string> PostAsync(string baseUri, string relativeUrl, object data)
        {
            return RequestAsync(HttpMethod.Post, baseUri, relativeUrl, data);
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(string baseUri, string relativeUrl, object data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await RequestAsync(HttpMethod.Get, baseUri, relativeUrl, data));
        }

        /// <inheritdoc />
        public async Task<T> PostAsync<T>(string baseUri, string relativeUrl, object data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await RequestAsync(HttpMethod.Post, baseUri, relativeUrl, data));
        }

        #endregion        

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~HttpClientWapper()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
