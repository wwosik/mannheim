using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client.RestApi;
using Mannheim.Salesforce.Client.RestApi.Describes;
using Mannheim.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public SalesforceClient(HttpClient httpClient, SalesforceOAuthToken salesforceToken, ILogger logger)
        {
            this.HttpClient = httpClient;
            this.SalesforceToken = salesforceToken;
            this.logger = logger;
            httpClient.BaseAddress = new Uri(salesforceToken.InstanceUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(salesforceToken.TokenType, salesforceToken.AccessToken);
        }

        public async Task<List<T>> QueryAnonymousTypeAndContinueAsync<T>(string queryText, T anonymousObjectOfType)
        {
            this.logger.LogTrace("Querying...");
            var result = new List<T>();
            var queryResult = await this.HttpClient.GetJsonAsync<QueryResult<T>>($"{this.ApiVersion.Url}/query?q={Uri.EscapeUriString(queryText)}");
            this.logger.LogTrace($"Got {queryResult.Records.Count} of {queryResult.TotalSize}.");

            result.AddRange(queryResult.Records);

            while (queryResult.NextRecordsUrl != null)
            {
                this.logger.LogTrace("Getting next batch...");
                queryResult = await this.QueryNextAsync<T>(queryResult.NextRecordsUrl);
                result.AddRange(queryResult.Records);
                this.logger.LogTrace($"Got {queryResult.Records?.Count} of {queryResult.TotalSize}.");
            }

            return result;
        }

        public async Task<QueryResult<T>> QueryAsync<T>(string queryText) where T : SObject
        {
            this.logger.LogTrace("Querying...");
            var result = await this.HttpClient.GetJsonAsync<QueryResult<T>>($"{this.ApiVersion.Url}/query?q={Uri.EscapeUriString(queryText)}");
            this.logger.LogTrace($"Got {result.Records.Count} of {result.TotalSize}.");
            return result;
        }

        public Task<QueryResult<T>> QueryNextAsync<T>(string nextRecordsUrl)
        {
            return this.HttpClient.GetJsonAsync<QueryResult<T>>(nextRecordsUrl);
        }

        public Task<List<ApiVersion>> GetApiVersionsAsync()
        {
            return this.HttpClient.GetJsonAsync<List<ApiVersion>>("/services/data");
        }

        public async Task<List<T>> QueryAndContinueAsync<T>(string queryText) where T : SObject
        {
            var queryResult = await this.QueryAsync<T>(queryText);
            var result = new List<T>(queryResult.TotalSize);
            result.AddRange(queryResult.Records);
            while (queryResult.NextRecordsUrl != null)
            {
                this.logger.LogTrace("Getting next batch...");
                queryResult = await this.QueryNextAsync<T>(queryResult.NextRecordsUrl);
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
            var versions = await this.HttpClient.GetJsonAsync<List<ApiVersion>>("/services/data");
            if (versions.Count == 0)
            {
                throw new Exception("No API versions received!");
            }

            this.ApiVersion = versions.OrderByDescending(v => v.Version).First();
        }

        public Task<DescribeSObjectsResult> DescribeObjectsAsync()
        {
            var resourcePath = $"{this.ApiVersion.Url}/sobjects";
            return this.HttpClient.GetJsonAsync<DescribeSObjectsResult>(resourcePath);
        }

        public Task<DescribeObjectResult> DescribeObjectAsync(string objectApiName)
        {
            var resourcePath = $"{this.ApiVersion.Url}/sobjects/{objectApiName}/describe";
            return this.HttpClient.GetJsonAsync<DescribeObjectResult>(resourcePath);
        }
    }
}
