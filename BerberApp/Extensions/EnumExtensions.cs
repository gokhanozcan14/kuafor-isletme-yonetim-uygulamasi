using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value == null)
            return string.Empty;

        var field = value.GetType().GetField(value.ToString());

        if (field == null)
            return value.ToString(); // Return the enum name if we can't find the field

        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        // If no description attribute exists, return the enum name
        return attribute?.Description ?? value.ToString();
    }
}
