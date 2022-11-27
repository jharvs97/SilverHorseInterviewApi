using SilverHorseInterviewApi.Aggregates;
using SilverHorseInterviewApi.Models;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace SilverHorseInterviewApi
{
    public class Program
    {
        static Uri BuildMockDataUri(string path)
        {
            return new UriBuilder("https", "jsonplaceholder.typicode.com", 443, path).Uri;
        }

        static string GetModelApiName<T>()
        {
            string resourceName = typeof(T).Name.ToLower();
            if (resourceName.EndsWith('y'))
            {
                resourceName = resourceName.Substring(0, resourceName.Length - 1) + "ies";
            }
            else
            {
                resourceName += "s";
            }
            return resourceName;
        }
        
        static string ModelToApiPath<T>(string apiPrefix)
        {
            return apiPrefix + GetModelApiName<T>();
        }

        static async Task<IEnumerable<T>> GetCollection<T>(Uri uri)
            where T : IModel
        {
            var json = await GetCollectionJson(uri);

            // Return a deserialized collection or an empty collection
            return JsonSerializer.Deserialize<IEnumerable<T>>(json) ?? new T[] { };
        }

        static async Task<string> GetCollectionJson(Uri uri)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetStringAsync(uri);
        }

        static void AddResourceEndpoint<T>(WebApplication app, string endpointPrefix = "api/")
            where T : IModel
        {
            Type type = typeof(T);
            string resourceCollectionPath = ModelToApiPath<T>(endpointPrefix);

            app.MapGet(resourceCollectionPath, async (HttpContext context) =>
            {
                return await GetCollection<T>(BuildMockDataUri(GetModelApiName<T>()));
            })
            .Produces(200, typeof(T), "application/json");
        }

        static void AddAggregateEndpoint(WebApplication app, string endpointPrefix = "api/")
        {
            app.MapGet(endpointPrefix + "collection", async (HttpContext context) =>
            {
                Collection collection = new Collection();
                collection.Posts  = (await GetCollection<Post>(BuildMockDataUri(GetModelApiName<Post>()))).ToArray();
                collection.Albums = (await GetCollection<Album>(BuildMockDataUri(GetModelApiName<Album>()))).ToArray();
                collection.Users  = (await GetCollection<User>(BuildMockDataUri(GetModelApiName<User>()))).ToArray();
                return collection;
            })
            .Produces(200, typeof(Collection), "application/json");
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            AddResourceEndpoint<Post>(app);
            AddResourceEndpoint<User>(app);
            AddResourceEndpoint<Album>(app);

            AddAggregateEndpoint(app);

            app.Run();
        }
    }
}