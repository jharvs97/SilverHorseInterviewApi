using SilverHorseInterviewApi.Models;

namespace SilverHorseInterviewApi.Store
{
    /// <summary>
    /// Interface to a backing store
    /// </summary>
    internal interface IStore
    {
        /// <summary>
        /// <para>Read all data that follows the schema T from the Store</para>
        /// <para>If nothing is found, an empty collection is returned</para>
        /// </summary>
        /// <typeparam name="T">Model schema</typeparam>
        /// <returns>A collection of models</returns>
        Task<IEnumerable<T>> ReadCollection<T>() where T : IModel;

        /// <summary>
        /// Read a single T that has the specified Id
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="id">Id</param>
        /// <exception cref="NotFoundInStoreException{T}"></exception>
        /// <returns></returns>
        Task<T> Read<T>(int id) where T : IModel;

        /// <summary>
        /// Write new data to the store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Write<T>(T model) where T : IModel;

        /// <summary>
        /// Overwrite data at the given Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Write<T>(int id, T model) where T : IModel;
    }
}
