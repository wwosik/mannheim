Mannheim.DataProtection
=======================

A simple, protected filesystem-based store.

A part of the Mannheim libraries.

Example usage
---------
   
1. Configure as service

```csharp
    services.AddDataProtection();

    services.Configure<DataProtectedFileStoreOptions>(o =>
    {
        var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var myFolder = Path.Combine(appDataFolder, "ProtectedFolder");
        o.Folder = new DirectoryInfo(myFolder);
        o.ProtectorKey = "CODE";
    });

    services.AddDataProtectedFileStore();
```

2. Consume

```csharp
    public class MyRepository {
        private readonly DataProtectedFileStore store;        

        public MyRepository(DataProtectedFileStore store) {
            this.store = store;
        }

        public Task StoreObjectSecurelyAsync(string name, MyObjectToBeSecured obj) {
            return this.store.SaveJsonAsync(name, obj);
        }

        public Task<MyObjectToBeSecured> GetSecurelyStoredObjectAsync(string name) {
            return this.store.LoadJsonAsync<MyObjectToBeSecured>(name);
        }
    }
```