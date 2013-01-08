using System;

namespace luckybird
{
    public static class Guard
    {
        public static void Against(bool condition, string paramName = null, string message = null)
        {
            if (condition)
                throw new ArgumentException(paramName, message);
        }
    }
}
