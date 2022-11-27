namespace SilverHorseInterviewApi.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// <para>Take N samples from an Enumerable</para>
        /// <para>Doesn't actually seed the Random class.</para>
        /// </summary>
        /// <typeparam name="T">Data type in the enumerable</typeparam>
        /// <param name="enumerable">Input enumerable</param>
        /// <param name="numSamples">Number of samples to be taken</param>
        /// <returns></returns>
        public static IEnumerable<T> Sample<T>(this IEnumerable<T> enumerable, int numSamples)
        {
            numSamples = Math.Min(numSamples, enumerable.Count());
            Random random = new Random();
            // Copy construct a list from the enumerable so we can pluck elements out as we select them randomly
            List<T> listCopy = new List<T>(enumerable);
            List<T> outList = new List<T>();

            for (int i = 0; i < numSamples; i++)
            {
                int index = random.Next(listCopy.Count() - 1);
                outList.Add(enumerable.ElementAt(index));
                listCopy.RemoveAt(index);
            }

            return outList;
        }
    }
}
