namespace Instaq.ImageProcessor.Standard
{
    using System;
    using System.Collections.Generic;

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }
    }
}
