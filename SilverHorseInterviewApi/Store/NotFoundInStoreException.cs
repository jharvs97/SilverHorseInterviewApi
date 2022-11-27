namespace SilverHorseInterviewApi.Store
{
    public class NotFoundInStoreException<T> : Exception
    {
        public NotFoundInStoreException(int id) : base($"Could not find Model {typeof(T).Name} with Id {id} in the store")
        {
        }
    }
}
