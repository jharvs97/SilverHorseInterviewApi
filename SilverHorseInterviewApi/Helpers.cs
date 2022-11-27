using SilverHorseInterviewApi.Models;

namespace SilverHorseInterviewApi
{
    internal static class Helpers
    {
        /// <summary>
        /// Transforms a model class name into an API name
        /// e.g. Post -> posts
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <returns></returns>
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
    }
}
