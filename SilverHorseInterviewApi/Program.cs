using Microsoft.AspNetCore.Authorization;
using SilverHorseInterviewApi.Aggregates;
using SilverHorseInterviewApi.Models;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using SilverHorseInterviewApi.Store;

namespace SilverHorseInterviewApi
{
    public class Program
    {
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

            var endpointBuilder = new ResourceEndpointBuilder(app, new MockStore());
            endpointBuilder.Add<Post>();
            endpointBuilder.Add<User>();
            endpointBuilder.Add<Album>();

            endpointBuilder.CreateAggregate<Collection>(async (IStore store) =>
            {
                var collection = new Collection();
                collection.Posts  = ChooseNRandom(await store.ReadCollection<Post>(), 30).ToArray();
                collection.Albums = ChooseNRandom(await store.ReadCollection<Album>(), 30).ToArray();
                collection.Users  = ChooseNRandom(await store.ReadCollection<User>(), 30).ToArray();

                return collection;
            });

            app.Run();
        }

        private static IEnumerable<T> ChooseNRandom<T>(IEnumerable<T> items, int n)
        {
            Random random = new Random();
            List<T> inItemsList = new List<T>(items);
            List<T> outList = new List<T>();
            
            for (int i = 0; i < n; i++)
            {
                int index = random.Next(inItemsList.Count());
                outList.Add(inItemsList.ElementAt(index));
                inItemsList.RemoveAt(index);
            }

            return outList;
        }
    }
}