using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> PostFormAndReceiveJsonAsync<TResult>(this HttpClient httpClient, Uri url, IDictionary<string, string> payload)
        {
            using var encodedPayload = new FormUrlEncodedContent(payload);
            var result = await httpClient.PostAsync(url, encodedPayload).ConfigureAwait(false);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Failed request with code {result.StatusCode} {result.ReasonPhrase}: {content}");
            }

            return JsonSerializer.Deserialize<TResult>(content);
        }

        public static async Task<TResult> GetJsonAsync<TResult>(this HttpClient httpClient, Uri url)
        {
            var result = await httpClient.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Failed request with code {result.StatusCode} {result.ReasonPhrase}: {content}");
            }

            return JsonSerializer.Deserialize<TResult>(content);
        }
    }
}
