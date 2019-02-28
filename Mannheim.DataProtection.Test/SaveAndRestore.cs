using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.DataProtection
{
    public class SaveAndRestore
    {
        private readonly TestingServices testingServices;

        public SaveAndRestore(ITestOutputHelper testOutputHelper)
        {
            this.testingServices = new TestingServices(testOutputHelper);
        }

        [Fact]
        public async Task SaveAndRestoreAsync()
        {
            var store = this.testingServices.GetRequiredService<DataProtectedFileStore>();
            var data = new Data { Field = "1" };
            await store.SaveJsonAsync("obj", data);

            var dataFromStore = await store.LoadJsonAsync<Data>("obj");

            Assert.Equal(data.Field, dataFromStore.Field);
        }

        public class Data
        {
            public string Field { get; set; }
        }
    }
}
