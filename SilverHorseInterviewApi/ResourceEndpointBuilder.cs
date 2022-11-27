using SilverHorseInterviewApi.Aggregates;
using SilverHorseInterviewApi.Models;
using SilverHorseInterviewApi.Store;

namespace SilverHorseInterviewApi
{
    /// <summary>
    /// Abstraction over adding CRUD operations on a Model
    /// </summary>
    internal class ResourceEndpointBuilder
    {
        private readonly WebApplication _app;
        private readonly IStore _store;

        /// <summary>
        /// Allow the user to create custom aggregates by giving them a reference to the IStore
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public delegate Task<IAggregate> AggregateBuilderDelegate(IStore store);

        public ResourceEndpointBuilder(WebApplication app, IStore store) 
        {
            _app = app;
            _store = store;
        }

        /// <summary>
        /// Add GRUD operations for a model
        /// </summary>
        /// <typeparam name="T">The Model</typeparam>
        /// <param name="endpointPath">Optional: Defaults the endpoint to the name of the Model</param>
        public void Add<T>(string? endpointPath = null) where T : IModel
        {
            string resourceCollectionPath;

            if (string.IsNullOrEmpty(endpointPath))
            {
                resourceCollectionPath = Helpers.GetModelApiName<T>();
            }
            else
            {
                resourceCollectionPath = endpointPath;
            }

            _app.MapGet(resourceCollectionPath, async (HttpContext context) =>
            {
                return await _store.ReadCollection<T>();
            })
            .Produces(200, typeof(T), "application/json");

            _app.MapGet(resourceCollectionPath + "/{id}", async (int id, HttpContext context) =>
            {
                return await _store.Read<T>(id);
            })
            .Produces(200, typeof(T), "application/json");

            _app.MapPost(resourceCollectionPath, async (T data, HttpContext context) =>
            {
                await _store.Write(data);
            });

            _app.MapPut(resourceCollectionPath + "/{id}", async (int id, T data, HttpContext context) =>
            {
                await _store.Write(id, data);
            });

        }

        /// <summary>
        /// Create an endpoint that returns an aggregate
        /// </summary>
        /// <typeparam name="T">The Aggregate type</typeparam>
        /// <param name="endpointPath">The endpoint for the api</param>
        /// <param name="aggregateBuilder">A delegate method that returns a T aggregate</param>
        public void CreateAggregate<T>(string endpointPath, AggregateBuilderDelegate aggregateBuilder) where T : IAggregate
        {
            _app.MapGet(endpointPath, async (HttpContext context) =>
            {
                return (T) await aggregateBuilder.Invoke(_store);
            });
        }
    }
}
