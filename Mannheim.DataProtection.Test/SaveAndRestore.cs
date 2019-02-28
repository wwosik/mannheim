using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mannheim.DataProtection
{
    public class SaveAndRestore
    {
        [Fact]
        public async Task SaveAndRestoreAsync()
        {
            var store = TestingServices.GetService<DataProtectedFileStore>();
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
