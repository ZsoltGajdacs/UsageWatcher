using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UsageWatcher.Enums
{
    /// <summary>
    /// Taken from: https://stackoverflow.com/a/19621488
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Extensible base method, can be used to get the attributes of an Enum
        /// </summary>
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "This is an extension method")]
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            Type type = value.GetType();
            System.Reflection.MemberInfo[] memberInfo = type.GetMember(value.ToString());
            object[] attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return attributes.Length > 0 ? (T)attributes[0] : null;
        }

        /// <summary>
        /// Retrieves the Description attribute of the given Enum
        /// </summary>
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "This is an extension method")]
        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Retrieves the DisplayAttribute.DisplayAttribute Name property
        /// </summary>
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "This is an extension method")]
        public static string GetDisplayName(this Enum value)
        {
            DisplayAttribute attribute = value.GetAttribute<DisplayAttribute>();
            return attribute == null ? value.ToString() : attribute.Name;
        }
    }
}
