using Microsoft.AspNetCore.Authorization;
using SilverHorseInterviewApi.Aggregates;
using SilverHorseInterviewApi.Models;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

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
            app.MapGet("/collection", async (HttpContext context) =>
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

            builder.Services.AddAuthorization();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // For testing with swagger, add Bearer token auth scheme.
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Custom middleware to check bearer token
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.Authorization != "af24353tdsfw")
                {
                    throw new Exception("User not authorized");
                }

                await next(context);
            });

            AddResourceEndpoint<Post>(app);
            AddResourceEndpoint<User>(app);
            AddResourceEndpoint<Album>(app);

            AddAggregateEndpoint(app);

            app.Run();
        }
    }
}