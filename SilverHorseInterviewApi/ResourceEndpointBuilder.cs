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

        public void Add<T>(string? endpointPath = null) where T : IModel
        {
            string resourceCollectionPath;

            if (string.IsNullOrEmpty(endpointPath))
            {
                Type type = typeof(T);
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

            // Create/Update are no-op as I dont have a _real_ db store.
            _app.MapPost(resourceCollectionPath + "/{id}", async (int id, T data, HttpContext context) =>
            {
                await _store.Write(data);
            });

            _app.MapPut(resourceCollectionPath + "/{id}", async (int id, T data, HttpContext context) =>
            {
                await _store.Write(data);
            });

        }

        public void CreateAggregate<T>(string endpointPath, AggregateBuilderDelegate del) where T : IAggregate
        {
            _app.MapGet(endpointPath, async (HttpContext context) =>
            {
                return (T) await del.Invoke(_store);
            });
        }
    }
}
