using SilverHorseInterviewApi.Aggregates;
using SilverHorseInterviewApi.Models;
using SilverHorseInterviewApi.Store;
using Microsoft.OpenApi.Models;
using System.Buffers;
using SilverHorseInterviewApi.Extensions;

namespace SilverHorseInterviewApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();

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
                            Scheme = "Bearer",
                            Name = "Authorization",
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
            app.UseAuthentication();
            
            // Custom middleware to check bearer token
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.Authorization != "af24353tdsfw")
                {
                    context.Response.StatusCode = 501;
                    await context.Response.WriteAsync("User not authenticated");
                }
                else
                {
                    await next(context);
                }
            });

            var endpointBuilder = new ResourceEndpointBuilder(app, new MockStore());
            endpointBuilder.Add<Post>("api/posts");
            endpointBuilder.Add<User>("api/users");
            endpointBuilder.Add<Album>("api/albums");

            endpointBuilder.CreateAggregate<Collection>("/collection", async (IStore store) =>
            {
                var collection = new Collection();
                collection.Posts  = (await store.ReadCollection<Post>()).Sample(30).ToArray();
                collection.Albums = (await store.ReadCollection<Album>()).Sample(30).ToArray();
                collection.Users  = (await store.ReadCollection<User>()).Sample(30).ToArray();

                return collection;
            });

            app.Run();
        }
    }
}