using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

using KashyyykFramework.Mobile.Helpers;

namespace KashyyykFramework.Mobile.Http
{
    public class WebApiProvider<T> : IDisposable
    {
        HttpClient _httpClient = null;
        string _baseUrl = null;
        string _servicePath = null;

        public WebApiProvider(string baseUrl, string servicePath)
        {
            this._baseUrl = baseUrl;
            this._servicePath = servicePath;
            this._httpClient = CreateHttpClient(this._baseUrl);
        }

        public IList<T> Get()
        {
            string responseContent = this.SendRequestSynchrone(this._httpClient.GetAsync, this._servicePath);

            if (responseContent != null)
            {
                IList<T> lst = JsonHelper.DeserializeJson<IList<T>>(responseContent);
                return lst;
            }

            return new List<T>();
        }

        public T Get(object value)
        {
            string uri = $"{this._servicePath}/{value.ToString()}";
            string responseContent = this.SendRequestSynchrone(this._httpClient.GetAsync, uri);

            if (responseContent != null)
            {
                T item = JsonHelper.DeserializeJson<T>(responseContent);
                return item;
            }

            return default(T);
        }

        public IList<T> Get(string addPath, IDictionary<string, object> urlParameters = null)
        {
            string uri = GetFullUri(addPath, urlParameters);

            string responseContent = this.SendRequestSynchrone(this._httpClient.GetAsync, uri);
            if (responseContent != null)
            {
                IList<T> lst = JsonHelper.DeserializeJson<IList<T>>(responseContent);
                return lst;
            }

            return new List<T>();
        }

        public TValue Get<TValue>(string addPath, IDictionary<string, object> urlParameters = null)
        {
            string uri = GetFullUri(addPath, urlParameters);

            string responseContent = this.SendRequestSynchrone(this._httpClient.GetAsync, uri);
            if (responseContent != null)
            {
                TValue val = JsonHelper.DeserializeJson<TValue>(responseContent);
                return val;
            }

            return default(TValue);
        }

        public T Post(T item)
        {
            HttpContent content = new StringContent(JsonHelper.SerializeJson(item), Encoding.UTF8, "application/json");
            string responseContent = this.SendRequestSynchrone(this._httpClient.PostAsync, this._servicePath, content);
            if (responseContent != null)
            {
                T newItem = JsonHelper.DeserializeJson<T>(responseContent);
                return newItem;
            }

            throw new Exception("Erreur de création de la ressource !");
        }

        public void Put(object idValue, T item)
        {
            string uri = $"{this._servicePath}/{idValue.ToString()}";
            HttpContent content = new StringContent(JsonHelper.SerializeJson(item), Encoding.UTF8, "application/json");
            string responseContent = this.SendRequestSynchrone(this._httpClient.PutAsync, uri, content);
            if (responseContent == null) throw new Exception("Erreur lors de la mise à jour de la ressource !");
        }

        public void Dispose()
        {
            this._httpClient?.Dispose();
        }

        #region Méthodes privées

        private HttpClient CreateHttpClient(string baseUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private string GetFullUri(string addPath, IDictionary<string, object> parameters)
        {
            string urlQueryString = null;
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var kvp in parameters) { sb.Append($"&{kvp.Key}={kvp.Value.ToString()}"); }
                urlQueryString = sb.ToString().Remove(0, 1);
            }
            string uri = (string.IsNullOrWhiteSpace(urlQueryString) ? $"{this._servicePath}/{addPath}" : $"{this._servicePath}/{addPath}?{urlQueryString}");

            return uri;
        }

        private string SendRequestSynchrone(Func<string, Task<HttpResponseMessage>> httpClientFunc, string uri)
        {
            Task<string> responseContent = Task.Run(async () =>
            {
                HttpResponseMessage response = await httpClientFunc(uri);
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = await response.Content.ReadAsStringAsync();
                    return responseStr;
                }
                return null;
            });

            return responseContent?.Result;
        }

        private string SendRequestSynchrone(Func<string, HttpContent, Task<HttpResponseMessage>> httpClientFunc, string uri, HttpContent content)
        {
            Task<string> responseContent = Task.Run(async () =>
            {
                HttpResponseMessage response = await httpClientFunc(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = await response.Content.ReadAsStringAsync();
                    return responseStr;
                }
                return null;
            });

            return responseContent?.Result;
        }

        #endregion
    }
}
