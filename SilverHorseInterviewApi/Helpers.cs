using SilverHorseInterviewApi.Models;

namespace SilverHorseInterviewApi
{
    internal static class Helpers
    {
        public static string GetModelApiName<T>() where T : IModel
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

        public static string ModelToApiPath<T>(string apiPrefix) where T : IModel
        {
            return apiPrefix + GetModelApiName<T>();
        }
    }
}
