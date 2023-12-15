using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class EnumDescription
{
    public static string GetDescriptionText<T>(this T source)
    {
        FieldInfo fi = source.GetType().GetField(source.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
         typeof(DescriptionAttribute), false);
        if (attributes.Length > 0) return attributes[0].Description;
        else return source.ToString();
    }

    public static string GetEnumTooltip<T>(this T value)
    {
        FieldInfo fi = typeof(T).GetField(value.ToString());
        TooltipAttribute[] attributes = fi.GetCustomAttributes(typeof(TooltipAttribute), false) as TooltipAttribute[];
        if (attributes != null && attributes.Length > 0)
            return attributes[0].tooltip;
        else
            return string.Empty;
    }

}
