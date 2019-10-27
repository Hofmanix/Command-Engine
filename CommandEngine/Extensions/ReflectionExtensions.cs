
using CommandEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommandEngine.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetCommandName(this Type commandType)
        {
            return GetName(commandType, "Command");
        }

        public static string GetAreaName(this Type areaType)
        {
            return GetName(areaType, "Area");
        }

        internal static string GetName(this Type type, string subtractFromEnd = "")
        {
            string typeName;
            var commandNameAttribute = type.GetCustomAttribute<NameAttribute>();
            if (commandNameAttribute != null)
            {
                typeName = commandNameAttribute.Name;
            }
            else
            {
                typeName = type.Name;
                if (!string.IsNullOrWhiteSpace(subtractFromEnd) && typeName.EndsWith(subtractFromEnd))
                {
                    typeName = typeName.Substring(0, typeName.Length - subtractFromEnd.Length);
                }
            }

            return typeName.ToLower();
        }

        internal static string[] GetCommandParameterNames(this ParameterInfo memberInfo)
        {
            var names = new List<string>();
            var nameAttribute = memberInfo.GetCustomAttribute<NameAttribute>();
            if (nameAttribute == null)
            {
                names.Add(memberInfo.Name);
            }
            else
            {
                names.Add(nameAttribute.Name);
                if (!string.IsNullOrWhiteSpace(nameAttribute.ShortName))
                {
                    names.Add(nameAttribute.ShortName);
                }
            }
            return names.ToArray();
        }
    }
}
