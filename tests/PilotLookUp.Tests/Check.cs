using System;

namespace PilotLookUp.Tests
{
    // Имя содержит "Assert" - хост классифицирует такие исключения как Failed (а не Error)
    public sealed class AssertFailedException : Exception
    {
        public AssertFailedException(string message) : base(message)
        {
        }
    }

    // Минимальные ассерты без внешних библиотек (в Tests-папку не тащим зависимости)
    public static class Check
    {
        public static void That(bool condition, string message)
        {
            if (!condition)
                throw new AssertFailedException(message);
        }

        public static void NotNull(object value, string what)
        {
            if (value == null)
                throw new AssertFailedException(what + " не должен быть null");
        }

        public static void NotEmpty(string value, string what)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new AssertFailedException(what + " не должен быть пустым");
        }

        public static void Contains(string haystack, string needle, string what)
        {
            if (haystack == null || haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) < 0)
                throw new AssertFailedException($"{what}: ожидалось вхождение '{needle}', получено '{haystack}'");
        }

        public static void Equal<T>(T expected, T actual, string what)
        {
            if (!Equals(expected, actual))
                throw new AssertFailedException($"{what}: ожидалось '{expected}', получено '{actual}'");
        }

        public static TException Throws<TException>(Action action, string what) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException e)
            {
                return e;
            }
            catch (Exception e)
            {
                throw new AssertFailedException($"{what}: ожидалось {typeof(TException).Name}, получено {e.GetType().Name}: {e.Message}");
            }

            throw new AssertFailedException($"{what}: ожидалось исключение {typeof(TException).Name}, но его не было");
        }
    }
}
