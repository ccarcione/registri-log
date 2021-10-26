using Microsoft.EntityFrameworkCore;
using System;

namespace net_registri_log.Shared.ExtensionMethods
{
    public static class StringExtension
    {
        public static bool Like(this string source, string value)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;
            return EF.Functions.Like(source, string.Concat("%", value, "%"));
        }

        /// <summary>
        /// Se questi enum danno problemi di prestazioni pensare di sostituirlo con un dizionario.
        /// https://stackoverflow.com/questions/16100/convert-a-string-to-an-enum-in-c-sharp/38711#38711
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
