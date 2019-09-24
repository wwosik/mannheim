using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client.RestApi;
using Mannheim.Salesforce.Client.RestApi.Describes;
using Mannheim.Utils;
using Microsoft.Extensions.Logging;

namespace Mannheim.Salesforce.Client
{
    public class SalesforceClient
    {
        private readonly ILogger logger;
        private ApiVersion apiVersion = new ApiVersion { Version = "44.0", Url = "/services/data/v44.0" };

        public HttpClient HttpClient { get; }
        public SalesforceOAuthToken SalesforceToken { get; }

        public ApiVersion ApiVersion
        {
            get { return this.apiVersion; }
            set
            {
                if (value == null || value.Url == null) throw new ArgumentException("Invalid version object!", nameof(this.ApiVersion));
                this.apiVersion = value;
            }
        }

        public SalesforceClient(HttpClient httpClient, SalesforceOAuthToken salesforceToken, ILogger<SalesforceClient> logger)
        {
            this.HttpClient = httpClient;
            this.SalesforceToken = salesforceToken;
            this.logger = logger;
            httpClient.BaseAddress = new Uri(salesforceToken.InstanceUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(salesforceToken.TokenType, salesforceToken.AccessToken);
        }

        public async Task UpdateAsync(string id, string objectType, object newVersion)
        {
            this.logger.LogTrace($"Updating {objectType} {id}");
            using var patchContent = new StringContent(JsonSerializer.Serialize(newVersion));
            patchContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this.HttpClient.PatchAsync(GetDataUri($"/sobjects/{objectType}/{id}"), patchContent);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new SalesforceException($"Failed request with code {response.StatusCode} {response.ReasonPhrase}: {content}");
            }
        }

        public async Task<IList<UpdateResult>> MassUpdateAsync(IEnumerable<object> objects, bool allOrNone = false)
        {
            var list = objects.ToList();
            this.logger.LogTrace($"Updating {list.Count} objects");
            var payload = new
            {
                allOrNone,
                records = list
            };

            string serializedPayload = JsonSerializer.Serialize(payload);
            using var patchContent = new StringContent(serializedPayload);
            patchContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var uri = GetDataUri($"/composite/sobjects");
            var response = await this.HttpClient.PatchAsync(uri, patchContent);

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new SalesforceException($"Failed request with code {response.StatusCode} {response.ReasonPhrase}: {content}");
            }

            return JsonSerializer.Deserialize<List<UpdateResult>>(content, this.JsonSerializerOptions);
        }

        private Uri GetDataUri(string resource)
        {
            return new Uri($"{this.ApiVersion.Url}{resource}", UriKind.Relative);
        }

        public async Task<List<T>> QueryAnonymousTypeAndContinueAsync<T>(string queryText, T anonymousObjectOfType)
        {
            this.logger.LogTrace("Querying...");
            var result = new List<T>();
            var queryResult = await this.HttpClient.GetJsonAsync<QueryResult<T>>(new Uri($"{this.ApiVersion.Url}/query?q={Uri.EscapeUriString(queryText)}", UriKind.Relative), this.JsonSerializerOptions);
            this.logger.LogTrace($"Got {queryResult.Records.Count} of {queryResult.TotalSize}.");

            result.AddRange(queryResult.Records);

            while (queryResult.NextRecordsUrl != null)
            {
                this.logger.LogTrace("Getting next batch...");
                queryResult = await this.QueryNextAsync<T>(new Uri(queryResult.NextRecordsUrl, UriKind.Relative));
                result.AddRange(queryResult.Records);
                this.logger.LogTrace($"Got {queryResult.Records?.Count} of {queryResult.TotalSize}.");
            }

            return result;
        }

        public async Task<QueryResult<T>> QueryAsync<T>(string queryText) where T : SObject
        {
            this.logger.LogTrace("Querying...");
            var uri = new Uri($"{this.ApiVersion.Url}/query?q={Uri.EscapeUriString(queryText)}", UriKind.Relative);
            var result = await this.HttpClient.GetJsonAsync<QueryResult<T>>(uri);
            this.logger.LogTrace($"Got {result.Records.Count} of {result.TotalSize}.");
            return result;
        }

        public async Task<QueryResult<T>> ToolingQueryAsync<T>(string queryText) where T : SObject
        {
            this.logger.LogTrace("Querying...");
            var uri = new Uri($"{this.ApiVersion.Url}/tooling/query?q={Uri.EscapeUriString(queryText)}", UriKind.Relative);
            var result = await this.HttpClient.GetJsonAsync<QueryResult<T>>(uri);
            this.logger.LogTrace($"Got {result.Records.Count} of {result.TotalSize}.");
            return result;
        }

        public Task<QueryResult<T>> QueryNextAsync<T>(Uri nextRecordsUrl)
        {
            return this.HttpClient.GetJsonAsync<QueryResult<T>>(nextRecordsUrl);
        }

        public Task<List<ApiVersion>> GetApiVersionsAsync()
        {
            return this.HttpClient.GetJsonAsync<List<ApiVersion>>(new Uri("/services/data", UriKind.Relative));
        }

        public async Task<List<T>> QueryAndContinueAsync<T>(string queryText) where T : SObject
        {
            var queryResult = await this.QueryAsync<T>(queryText);
            var result = new List<T>(queryResult.TotalSize);
            result.AddRange(queryResult.Records);
            while (queryResult.NextRecordsUrl != null)
            {
                this.logger.LogTrace("Getting next batch...");
                queryResult = await this.QueryNextAsync<T>(new Uri(queryResult.NextRecordsUrl, UriKind.Relative));
                result.AddRange(queryResult.Records);
                this.logger.LogTrace($"Got {result.Count} of {queryResult.TotalSize}.");

            }
            return result;
        }

        /// <summary>
        /// Queries available API versions and sets to the newest one
        /// </summary>
        /// <returns></returns>
        public async Task SetToUseNewestApiVersionAsync()
        {
            var versions = await this.HttpClient.GetJsonAsync<List<ApiVersion>>(new Uri("/services/data", UriKind.Relative));
            if (versions.Count == 0)
            {
                throw new Exception("No API versions received!");
            }

            this.ApiVersion = versions.OrderByDescending(v => v.Version).First();
        }

        public Task<DescribeSObjectsResult> DescribeObjectsAsync()
        {
            var resourcePath = new Uri($"{this.ApiVersion.Url}/sobjects", UriKind.Relative);
            return this.HttpClient.GetJsonAsync<DescribeSObjectsResult>(resourcePath);
        }

        public Task<DescribeObjectResult> DescribeObjectAsync(string objectApiName)
        {
            var resourcePath = new Uri($"{this.ApiVersion.Url}/sobjects/{objectApiName}/describe", UriKind.Relative);
            return this.HttpClient.GetJsonAsync<DescribeObjectResult>(resourcePath);
        }

        public async Task<TResult> CallApexService<TResult>(HttpMethod method, Uri uri, object payloadToJsonSerialize)
        {
            var content = await CallApexService(method, uri, payloadToJsonSerialize);

            var deserializedResult = JsonSerializer.Deserialize<TResult>(content, this.JsonSerializerOptions);
            return deserializedResult;
        }

        public async Task<string> CallApexService(HttpMethod method, Uri uri, object payloadToJsonSerialize)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri($"/services/apexrest{uri}", UriKind.Relative)
            };

            using StringContent payloadContent = payloadToJsonSerialize == null ? null : new StringContent(JsonSerializer.Serialize(payloadToJsonSerialize));

            if (payloadContent != null)
            {
                request.Content = payloadContent;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var result = await this.HttpClient.SendAsync(request);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Failed request with code {result.StatusCode} {result.ReasonPhrase}: {content}");
            }

            return content;
        }

        public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public class UpdateResult
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string StatusCode { get; set; }
        public List<string> Fields { get; set; }
        public string Message { get; set; }
    }
}
