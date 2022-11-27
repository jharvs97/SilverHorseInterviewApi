using SilverHorseInterviewApi.Models;

namespace SilverHorseInterviewApi.Store
{
    /// <summary>
    /// Interface to a backing store
    /// </summary>
    internal interface IStore
    {
        Task<IEnumerable<T>> ReadCollection<T>() where T : IModel;
        Task<T> Read<T>(int id) where T : IModel;
        Task Write<T>(T model) where T : IModel;
        Task Write<T>(int id, T model) where T : IModel;
    }
}
