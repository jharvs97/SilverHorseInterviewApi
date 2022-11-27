using SilverHorseInterviewApi.Aggregates;
using SilverHorseInterviewApi.Models;
using SilverHorseInterviewApi.Store;
using System.Runtime.CompilerServices;

namespace SilverHorseInterviewApi
{
    internal class ResourceEndpointBuilder
    {

        private readonly WebApplication _app;
        private readonly IStore _store;
        private readonly string _endpointPrefix;

        public delegate Task<IAggregate> AggregateBuilderDelegate(IStore store);

        public ResourceEndpointBuilder(WebApplication app, IStore store, string endpointPrefix = "app/") 
        {
            _app = app;
            _store = store;
            _endpointPrefix = endpointPrefix;
        }

        public void Add<T>() where T : IModel
        {
            Type type = typeof(T);
            string resourceCollectionPath = Helpers.ModelToApiPath<T>(_endpointPrefix);

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

            //TODO: Create and Update
        }

        public void CreateAggregate<T>(AggregateBuilderDelegate del) where T : IAggregate
        {
            _app.MapGet("/" + typeof(T).Name.ToLower(), async (HttpContext context) =>
            {
                return (T) await del.Invoke(_store);
            });
        }
    }
}
