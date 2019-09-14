using Mannheim.Salesforce.Client.RestApi;
using System.Threading.Tasks;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public interface ISalesforceClientFactory<T>
    {
        Task<T> CreateClientAsync(string name, ApiVersion apiVersion = null);
    }
}
