using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client.RestApi;

namespace Mannheim.Salesforce.Client
{
    public class SalesforceClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public SalesforceClient(HttpClient httpClient, SalesforceOAuthToken salesforceToken, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            httpClient.BaseAddress = new Uri(salesforceToken.InstanceUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(salesforceToken.TokenType, salesforceToken.AccessToken);
        }

        public async Task<List<T>> QueryAnonymousTypeAndContinueAsync<T>(string queryText, T anonymousObjectOfType)
        {
            this.logger.LogTrace("Querying...");
            var result = new List<T>();
            var queryResult = await this.httpClient.GetJsonAsync<QueryResult<T>>("/services/data/v44.0/query?q=" + Uri.EscapeUriString(queryText));
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
            var result = await this.httpClient.GetJsonAsync<QueryResult<T>>("/services/data/v44.0/query?q=" + Uri.EscapeUriString(queryText));
            this.logger.LogTrace($"Got {result.Records.Count} of {result.TotalSize}.");
            return result;
        }

        public Task<QueryResult<T>> QueryNextAsync<T>(string nextRecordsUrl)
        {
            return this.httpClient.GetJsonAsync<QueryResult<T>>(nextRecordsUrl);
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
    }
}
