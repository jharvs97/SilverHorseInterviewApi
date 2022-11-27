using SilverHorseInterviewApi.Models;
using System.Net;
using System.Net.Http.Headers;

namespace SilverHorseInterviewApi
{
    public class Program
    {
        static Uri BuildUri(string path)
        {
            return new UriBuilder("https", "jsonplaceholder.typicode.com", 443, path).Uri;
        }

        static async Task<T[]> GetCollection<T>(string path)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var uri = BuildUri(path);
            var json = await client.GetStringAsync(BuildUri(path));

            // Return a deserialized collection or an empty collection
            return JsonSerializer.Deserialize<T[]>(json) ?? new T[] { };
        }

        static void BuildResourceCollectionEndpoint<T>(WebApplication app, string endpointPrefix = "api/")
        {
            Type type = typeof(T);
            string resourceCollectionName = type.Name.ToLower() + "s";
            string resourceCollectionPath = endpointPrefix + resourceCollectionName;

            app.MapGet(resourceCollectionPath, async (HttpContext context) =>
            {
                return await GetCollection<T>(resourceCollectionName);
            });
        }

        static void BuildAggregateCollectionEndpoint<T>()
        {

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

            BuildResourceCollectionEndpoint<Post>(app);
            BuildResourceCollectionEndpoint<User>(app);
            BuildResourceCollectionEndpoint<Album>(app);

            app.Run();
        }
    }
}