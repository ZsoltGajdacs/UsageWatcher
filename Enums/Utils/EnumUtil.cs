using System;
using System.Collections.Generic;
using System.Linq;

namespace UsageWatcher.Enums.Utils
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static EnumMatchResult<T> GetEnumForString<T>(string enumDisplayNameOrDescription) where T : Enum
        {
            IEnumerable<T> enumElements = GetValues<T>();

            EnumMatchResult<T> result = null;
            foreach (T enumElem in enumElements)
            {
                if (enumElem.GetDisplayName() == enumDisplayNameOrDescription || 
                    enumElem.GetDescription() == enumDisplayNameOrDescription)
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
