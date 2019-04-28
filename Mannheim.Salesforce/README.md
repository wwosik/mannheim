Mannheim.Salesforce
=======================

Utilities for accessing Salesforce in non-interactive scenarios.

A part of the Mannheim libraries.


RexisClientProvider
-------------------

Creates clients using refresh tokens in an in-built token store. This allows accessing Salesforce 
either non-interactively or with a technical account.

1. Configure service
```csharp

services.Configure<SalesforceConfigDataProtectedFileStoreOptions>(o =>
{
    o.Folder = new DirectoryInfo("<<folder path>>");
    o.ProtectorKey = "Key";
});

services.AddDataProtection();
services.AddSingleton<ISalesforceConfigStore, SalesforceConfigDataProtectedFileStore>();
services.AddSingleton<SalesforceClientProvider>();
```

2. Provide OAuth configuration

```csharp
ISalesforceConfigStore store = <<from dependency injection>>;
var config = new SalesforceOAuthConfiguration {
    ConsumerId = "<<from Salesforce>>"
    , ConsumerSecret = "<<from Salesforce>>"
    , RedirectUrl = "<<from Salesforce>>"
};

await store.SaveSalesforceOAuthConfigurationAsync(config);
```

This needs to be done just once. Later on the config will be stored permanently in the store.

3. Get OAuth web flow url

```csharp
SalesforceClientProvider clientProvider = <<from dependency injection>>;
var salesforceLoginUrl = await clientProvider.GetWebFlowUriAsync(new Uri("https://address.my.salesforce.com"));
```

Alternatively use Device Flow.

4. Complete web flow and client

```csharp
SalesforceClientProvider clientProvider = <<from dependency injection>>;
string code = "<<from redirect>>";

await clientProvider.AddClientForTokenAsync(new Uri("https://address.my.salesforce.com"), code);
```

5. Use client where needed

```csharp
SalesforceClientProvider clientProvider = <<from dependency injection>>;
SalesforceClient client = await clientProvider.GetClientAsync();
```