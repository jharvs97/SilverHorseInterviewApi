using SilverHorseInterviewApi.Models;
using System.Net.Http.Headers;

namespace SilverHorseInterviewApi.Store
{
    /// <summary>
    /// A mock data store, wasn't sure if EF was allowed for the interview.
    /// The mock store no-ops all writes
    /// </summary>
    public class MockStore : IStore
    {
        private readonly Uri _mockDataUri = new UriBuilder("https", "jsonplaceholder.typicode.com").Uri;
        private HttpClient _httpClient;

        public MockStore()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _mockDataUri;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        async Task<IEnumerable<T>> IStore.ReadCollection<T>()
        {
            return await GetMockCollection<T>();
        }

        async Task<T> IStore.Read<T>(int id)
        {
            T? model = await GetMockRecord<T>(id);

            if (model == null)
            {
                throw new NotFoundInStoreException<T>(id);
            }

            return model;
        }

        async Task IStore.Write<T>(T model)
        {
            return;
        }

        async Task IStore.Write<T>(int id, T model)
        {
            return;
        }

        private async Task<IEnumerable<T>> GetMockCollection<T>()
           where T : IModel
        {
            var json = await GetJsonCollection(Helpers.GetModelApiName<T>());

            // Return a deserialized collection or an empty collection
            return JsonSerializer.Deserialize<IEnumerable<T>>(json) ?? new T[] { };
        }

        private async Task<T?> GetMockRecord<T>(int id)
           where T : IModel
        {
            var json = await GetJsonRecord(Helpers.GetModelApiName<T>(), id);

            return JsonSerializer.Deserialize<T>(json);
        }

        private async Task<string> GetJsonCollection(string path)
        {
            return await _httpClient.GetStringAsync(path);
        }

        private async Task<string> GetJsonRecord(string path, int id)
        {
            return await _httpClient.GetStringAsync(path + "/" + id);
        }
    }
}
