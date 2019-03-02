using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mannheim.Utils
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> PostFormAndReceiveJsonAsync<TResult>(this HttpClient httpClient, string url, IDictionary<string, string> payload)
        {
            var result = await httpClient.PostAsync(url, new FormUrlEncodedContent(payload));
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Failed request with code {result.StatusCode} {result.ReasonPhrase}: {content}");
            }

            return JsonConvert.DeserializeObject<TResult>(content);
        }

        public static async Task<TResult> GetJsonAsync<TResult>(this HttpClient httpClient, string url)
        {
            var result = await httpClient.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Failed request with code {result.StatusCode} {result.ReasonPhrase}: {content}");
            }

            return JsonConvert.DeserializeObject<TResult>(content);
        }
    }
}
