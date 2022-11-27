using SilverHorseInterviewApi.Models;
using System.IO;
using System.Net.Http.Headers;

namespace SilverHorseInterviewApi.Store
{
    public class MockStore : IStore
    {
        private readonly Uri _mockDataUri = new UriBuilder("https", "jsonplaceholder.typicode.com").Uri;

        public MockStore()
        {
        }

        async Task<IEnumerable<T>> IStore.ReadCollection<T>()
        {
            return await GetCollection<T>(Helpers.GetModelApiName<T>());
        }

        async Task<T> IStore.Read<T>(int id)
        {
            return await Get<T>(Helpers.GetModelApiName<T>(), id);
        }

        async Task IStore.Write<T>(T model)
        {
            return;
        }

        async Task IStore.Write<T>(int id, T model)
        {
            return;
        }

        private async Task<IEnumerable<T>> GetCollection<T>(string path)
           where T : IModel
        {
            var json = await GetJsonCollection(path);

            // Return a deserialized collection or an empty collection
            return JsonSerializer.Deserialize<IEnumerable<T>>(json) ?? new T[] { };
        }

        private async Task<T> Get<T>(string path, int id)
           where T : IModel
        {
            var json = await GetJson(path, id);

            return JsonSerializer.Deserialize<T>(json);
        }

        private async Task<string> GetJsonCollection(string path)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetStringAsync(new Uri(_mockDataUri, path));
        }

        private async Task<string> GetJson(string path, int id)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetStringAsync(new Uri(_mockDataUri, path + "/" + id));
        }
    }
}
