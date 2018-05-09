using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public static class ObjectTool
{
    public static Type getMemberType(this object obj, string memberName)
    {
        Type type = obj.GetType();
        var member = type.GetMember(memberName);
        if (member == null || member.Length == 0)
            return null;
        switch (member[0].MemberType)
        {
            case MemberTypes.Field:
                return type.GetField(memberName).FieldType;
            case MemberTypes.Property:
                return type.GetProperty(memberName).PropertyType;
            default:
                return null;
        }
    }

    public static object getMemberValue(this object obj, string memberName,
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        Type type = obj.GetType();
        var member = type.GetMember(memberName, bindingFlags);

        while (member == null || member.Length == 0)
        {
            type = type.BaseType;
            if (type == null)
                return null;

            member = type.GetMember(memberName, bindingFlags);
        }

        switch (member[0].MemberType)
        {
            case MemberTypes.Field:
                return type.GetField(memberName, bindingFlags).GetValue(obj);
            case MemberTypes.Property:
                return type.GetProperty(memberName, bindingFlags).GetValue(obj, null);
            default:
                return null;
        }
    }

    public static void setMemberValue(this object obj, string memberName, object value, 
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        Type type = obj.GetType();
        var member = type.GetMember(memberName, bindingFlags);

        while (member == null || member.Length == 0)
        {
            type = type.BaseType;
            if (type == null)
                return;

            member = type.GetMember(memberName, bindingFlags);
        }

        switch (member[0].MemberType)
        {
            case MemberTypes.Field:
                var field = type.GetField(memberName, bindingFlags);
                field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
                break;
            case MemberTypes.Property:
                var property = type.GetProperty(memberName, bindingFlags);
                property.SetValue(obj, Convert.ChangeType(value, property.PropertyType), null);
                break;
        }
    }

    public static object Call(this object obj, string funcName, object[] param, 
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        Type[] types = new Type[param.Length];
        for (int i = 0; i < param.Length; i++)
        {
            types[i] = param[i].GetType();
        }

        Type type = obj.GetType();
        MethodInfo mi = type.GetMethod(funcName, bindingFlags, null, types, null);

        while (mi == null)
        {
            type = type.BaseType;
            if (type == null)
                return null;

            mi = type.GetMethod(funcName, bindingFlags, null, types, null);
        }

        return mi.Invoke(obj, bindingFlags, null, param, null);//调用方法
    }

    public static object createList(Type type)
    {
        Type listType = typeof(List<>);
        Type specificType = listType.MakeGenericType(new System.Type[] { type });
        return Activator.CreateInstance(specificType, new object[] { });
    }

    public static int ToInt(this object obj)
    { return (int)Convert.ToDouble(obj); }

    public static uint ToUInt(this object obj)
    { return (uint)Convert.ToDouble(obj); }

    public static float ToFloat(this object obj)
    { return Convert.ToSingle(obj); }

    public static double ToDouble(this object obj)
    { return Convert.ToDouble(obj); }

    public static bool ToBool(this object obj)
    { return Convert.ToBoolean(obj); }

    public static bool ToBool(this int obj)
    { return Convert.ToBoolean(obj); }

    public static bool ToBool(this uint obj)
    { return Convert.ToBoolean(obj); }

    public static ulong ToULong(this object obj)
    { return (uint)Convert.ToUInt64(obj); }
}