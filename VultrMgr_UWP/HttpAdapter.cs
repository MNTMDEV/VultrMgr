using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace VultrMgr
{
    class HttpAdapter
    {
        private UserConfig userConfig;
        private string ApiKey;
        private HttpClient httpClient;
        private HttpRequestHeaderCollection httpHeader;
        public Uri uri { get; set; }
        private HttpResponseMessage httpResponse;
        private bool isSuccess;
        private IHttpContent httpContent;
        public List<KeyValuePair<string,string>> httpPair
        {
            set
            {
                httpContent = new HttpFormUrlEncodedContent(value);
            }
        }

        public HttpAdapter()
        {
            userConfig=((App)Application.Current).GetUserConfig();
            ApiKey = userConfig.ApiKey;
            httpClient = null;
            httpHeader = null;
            uri = null;
            httpResponse = null;
            isSuccess = false;
            //httpContent = null;
            InitHttp();
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="httpClient"></param>
        public void InitHttp()
        {
            httpClient = new Windows.Web.Http.HttpClient();
            httpHeader=httpClient.DefaultRequestHeaders;
        }

        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool AddHeader(KeyValuePair<string,string> pair)
        {
            if (httpHeader == null)
                return false;
            httpHeader.Add(pair);
            return true;
        }

        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="hName"></param>
        /// <param name="hValue"></param>
        /// <returns></returns>
        public bool AddHeader(string hName,string hValue)
        {
            return AddHeader(new KeyValuePair<string, string>(hName, hValue));
        }

        /// <summary>
        /// 清空请求头
        /// </summary>
        /// <returns></returns>
        public bool ClearHeader()
        {
            if (httpHeader == null)
                return false;
            httpHeader.Clear();
            return true;
        }

        /// <summary>
        /// URL编码函数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string UrlEncode(string str)
        //{
        //    string ret = WebUtility.UrlEncode(str).Replace("+", "%20");
        //    return ret;
        //}

        /// <summary>
        /// 将表单集合转化为经过URL编码的字符串
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        //public static string ConvertFormDataToString(NameValueCollection collection)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    foreach (KeyValuePair<string, string> item in collection)
        //    {
        //        builder.Append("&");
        //        builder.Append(UrlEncode(item.Key));
        //        builder.Append("=");
        //        builder.Append(UrlEncode(item.Value));
        //    }
        //    string strResult = builder.ToString();
        //    if (strResult.Length > 0)
        //        strResult = strResult.Substring(1);
        //    return strResult;
        //}

        /// <summary>
        /// 执行GET请求
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(Uri uri)
        {
            this.uri = uri;
            return await GetAsync();
        }

        /// <summary>
        /// 执行GET请求
        /// </summary>
        /// <returns>响应正文</returns>
        public async Task<string> GetAsync()
        {
            isSuccess = false;
            if (httpClient == null)
                return null;
            if (uri == null)
                return null;
            string strResult = "";
            try
            {
                //发送GET请求 
                httpResponse = await httpClient.GetAsync(this.uri);
                isSuccess = httpResponse.IsSuccessStatusCode;
                strResult = await httpResponse.Content.ReadAsStringAsync();
                DisposeResponse();
                return strResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(Uri uri, List<KeyValuePair<string,string>> pair)
        {
            this.uri = uri;
            this.httpPair = pair;
            return await PostAsync();
        }


        /// <summary>
        /// 执行POST请求(x-www-form-urlencode)
        /// </summary>
        /// <returns></returns>
        public async Task<string> PostAsync()
        {
            isSuccess = false;
            if (httpClient == null)
                return null;
            if (uri == null)
                return null;
            if (httpContent == null)
                return null;
            string strResult = "";
            try
            {
                //发送POST请求 
                httpResponse = await httpClient.PostAsync(this.uri, this.httpContent);
                isSuccess = httpResponse.IsSuccessStatusCode;
                strResult = await httpResponse.Content.ReadAsStringAsync();
                DisposeResponse();
            }
            catch (Exception)
            {
                ;
            }
            return strResult;
        }

        //<start>实际操作</start>

        /// <summary>
        /// 添加APIKEY请求头
        /// </summary>
        /// <returns></returns>
        public bool AddApiKeyHeader()
        {
            return AddHeader("API-Key", this.ApiKey);
        }

        /// <summary>
        /// POST的SUBID数据包
        /// </summary>
        /// <param name="subid"></param>
        public void SetSubidData(string subid)
        {
            List<KeyValuePair<string, string>> pair = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("SUBID",subid)
            };
            this.httpPair = pair;
        }

        /// <summary>
        /// 设置调用Vultr的Uri
        /// </summary>
        /// <param name="path"></param>
        public void SetApiUri(string path)
        {
            this.uri = new Uri("https://api.vultr.com" + path);
        }

        /// <summary>
        /// 密钥有效性
        /// </summary>
        /// <returns></returns>
        public async Task<bool> KeyValidAsync()
        {
            SetApiUri("/v1/auth/info");
            ClearHeader();
            AddApiKeyHeader();
            string strResult = await GetAsync();
            if (strResult == null)
                return false;
            if (!isSuccess)
                return false;
            return true;
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ServerInfo>> GetServerList()
        {
            SetApiUri("/v1/server/list");
            ClearHeader();
            AddApiKeyHeader();
            string strResult = await GetAsync();
            List<ServerInfo> infoRes = JsonHandle.ServerListJson(strResult);
            return infoRes;
        }

        /// <summary>
        /// 关机操作
        /// </summary>
        /// <param name="subid"></param>
        /// <returns></returns>
        public async Task<bool> ServerPowerOff(string subid)
        {
            SetApiUri("/v1/server/halt");
            SetSubidData(subid);
            ClearHeader();
            AddApiKeyHeader();
            string strResult = await PostAsync();
            return isSuccess;
        }

        /// <summary>
        /// 重启服务器
        /// </summary>
        /// <param name="subid">服务器ID</param>
        /// <returns>是否成功</returns>
        public async Task<bool> ServerRestart(string subid)
        {
            SetApiUri("/v1/server/reboot");
            SetSubidData(subid);
            ClearHeader();
            AddApiKeyHeader();
            string strResult = await PostAsync();
            return isSuccess;
        }

        /// <summary>
        /// 重装服务器
        /// </summary>
        /// <param name="subid"></param>
        /// <returns></returns>
        public async Task<bool> ServerReInstall(string subid)
        {
            SetApiUri("/v1/server/reinstall");
            SetSubidData(subid);
            ClearHeader();
            AddApiKeyHeader();
            string strResult = await PostAsync();
            return isSuccess;
        }

        /// <summary>
        /// 销毁服务器
        /// </summary>
        /// <param name="subid"></param>
        /// <returns></returns>
        public async Task<bool> ServerDestroy(string subid)
        {
            SetApiUri("/v1/server/destroy");
            SetSubidData(subid);
            ClearHeader();
            AddApiKeyHeader();
            string strResult = await PostAsync();
            return isSuccess;
        }

        //<end>实际操作</end>

        /// <summary>
        /// 销毁httpClient
        /// </summary>
        private void DisoseHttp()
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }
        }

        /// <summary>
        /// 销毁响应对象
        /// </summary>
        private void DisposeResponse()
        {
            if (httpResponse != null)
            {
                httpResponse.Dispose();
                httpResponse = null;
            }
        }

        /// <summary>
        /// 销毁请求正文缓冲区(不应直接调用)
        /// </summary>
        private void DisposeContent()
        {
            if (httpContent != null)
            {
                httpContent.Dispose();
                httpContent = null;
            }
        }

        /// <summary>
        /// 销毁该对象
        /// </summary>
        public void Dispose()
        {
            DisposeResponse();
            //DisposeContent();
            DisoseHttp();
        }
    }
    class NameValueCollection : List<KeyValuePair<string,string>>
    {
    }
}
