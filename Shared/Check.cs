using System;
using GraphCache.Object;

namespace GraphCache
{
    public static class Check
    {
        internal static string NotNullOrWhiteSpace(string text, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(parameterName);
            }

            return text;
        }

        internal static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}