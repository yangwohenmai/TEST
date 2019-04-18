using System;
using System.ComponentModel;
using System.Reflection;
using static System.String;

namespace Dapper.Net.Extensions {

    /// <summary>
    /// Helper extension methods for working with enum types.
    /// </summary>
    public static class EnumExtensions {
        /// <summary>
        /// Get the description decorated attribute string from an enum or its ToString() representation.
        /// </summary>
        /// <param name="item">The enum to get the string description of.</param>
        /// <returns>The string description.</returns>
        public static string GetDescription(this Enum item) {
            if (item == null) return Empty;
            var t = item.GetType();
            var memInfo = t.GetMember(item.ToString());
            if (memInfo.Length <= 0) return item.ToString();
            var attr = memInfo[0].GetCustomAttribute<DescriptionAttribute>(false);
            return attr?.Description ?? item.ToString();
        }

        public static T FirstValue<T>() => (T) Enum.GetValues(typeof (T)).GetValue(0);

        public static T GetEnum<T>(string stringValue, T defaultVal) where T : struct {
            var result = defaultVal;
            if (IsNullOrWhiteSpace(stringValue)) return result;
            if (Enum.TryParse(stringValue, true, out result) == false) result = defaultVal;
            if (Enum.IsDefined(typeof (T), result) == false) result = defaultVal;
            return result;
        }

        public static T Parse<T>(string value) where T : struct {
            if (IsNullOrWhiteSpace(value)) return default(T);
            T outVal;
            return Enum.TryParse(value, true, out outVal) ? outVal : default(T);
        }
    }

}
