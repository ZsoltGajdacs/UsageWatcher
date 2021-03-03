using System;
using System.Collections.Generic;
using System.Linq;

namespace UsageWatcher.Enums.Utils
{
    public static class EnumUtil
    {
        /// <summary>
        /// GIves back the given Enum's values as an Enum list
        /// </summary>
        /// <typeparam name="T">The enum whose values are to be listed</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Given the name, description or enum value it finds the enum of the given type
        /// </summary>
        /// <typeparam name="T">The found enum</typeparam>
        /// <param name="enumDisplayNameOrDescription">A string matching the name, description or as a last resort, the enum itself</param>
        /// <returns></returns>
        public static EnumMatchResult<T> GetEnumForString<T>(string enumDisplayNameOrDescription) where T : Enum
        {
            IEnumerable<T> enumElements = GetValues<T>();

            EnumMatchResult<T> result = null;
            foreach (T enumElem in enumElements)
            {
                if (enumElem.GetDisplayName() == enumDisplayNameOrDescription || 
                    enumElem.GetDescription() == enumDisplayNameOrDescription ||
                    enumElem.ToString() == enumDisplayNameOrDescription)
                {
                    result = new EnumMatchResult<T>(enumElem);
                }
            }

            return result;
        }
    }

    public class EnumMatchResult<T> where T : Enum
    {
        public T FoundEnum { get; private set; }

        public EnumMatchResult(T foundEnum)
        {
            FoundEnum = foundEnum;
        }
    }
}
