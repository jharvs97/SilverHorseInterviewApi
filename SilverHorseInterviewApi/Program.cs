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

        static async Task<string> GetData(string path)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var uri = BuildUri(path);
            return await client.GetStringAsync(BuildUri(path));
        }

        public static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

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

            app.MapGet("api/posts", async (HttpContext context) =>
            {
                return await GetData("posts");
            })
            .WithName("GetPosts");

            app.MapGet("api/users", async (HttpContext context) =>
            {
                return await GetData("users");
            })
            .WithName("GetUsers");

            app.MapGet("api/albums", async (HttpContext context) =>
            {
                return await GetData("albums");
            })
            .WithName("GetAlbums");

            app.MapGet("api/collection", async (HttpContext context) =>
            {

            })
            .WithName("GetCollection");

            app.Run();
        }
    }
}