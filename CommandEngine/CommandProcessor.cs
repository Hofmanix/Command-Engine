using CommandEngine.Attributes;
using CommandEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandEngine
{
    internal static class CommandProcessor
    {
        internal static MemberInfo GetNamedMember(ICommand command, string parameter) => command.GetType().GetMembers().FirstOrDefault(member => member.GetCustomAttribute<NameAttribute>() != null &&
                    member.GetCustomAttribute<NameAttribute>().Name.Equals(parameter, StringComparison.CurrentCultureIgnoreCase));

        internal static object[] ConvertMethodParameters(MethodInfo method, string[] parameters, MethodParameterNaming parameterNaming)
        {
            var methodParameters = method.GetParameters();

            if (!parameters.Any() && (!methodParameters.Any() || methodParameters.All(param => param.HasDefaultValue))) return Array.Empty<object>();
            if (methodParameters.Length == 1 && methodParameters[0].ParameterType == typeof(string[])) return new[] { parameters };
            if (!parameters.Any() && methodParameters.Where(parameter => !parameter.HasDefaultValue).Any()) throw new TargetParameterCountException($"No parameters for method with parameters without default value: {method.Name}");

            if (parameterNaming == MethodParameterNaming.Order)
            {
                if (parameters.Length > methodParameters.Length || parameters.Length < methodParameters.Where(param => !param.HasDefaultValue).Count()) throw new TargetParameterCountException($"Parameters can't cover method parameters for method {method.Name}");
                return parameters.Select((param, i) => methodParameters[i].ParameterType == typeof(string) ? param : Convert.ChangeType(param, methodParameters[i].ParameterType)).ToArray();
            }
            else
            {
                return methodParameters.Select(methodParam =>
                {
                    var names = methodParam.GetCommandParameterNames();
                    var paramName = parameters.FirstOrDefault(p => names.Any(name => name.Equals(p, StringComparison.CurrentCultureIgnoreCase)));

                    if (paramName == null && methodParam.HasDefaultValue) return methodParam.DefaultValue;
                    if (paramName != null)
                    {
                        var paramIndex = Array.IndexOf(parameters, paramName) + 1;
                        if (parameters.Length >= paramIndex) throw new TargetParameterCountException($"Parameters array doesn't contain specific parameter for parameter name {paramName}.");

                        var param = parameters[paramIndex];
                        if (methodParam.ParameterType == typeof(string)) return param;
                        return Convert.ChangeType(param, methodParam.ParameterType);
                    }

                    throw new TargetParameterCountException($"Parameter {paramName} not found in parameters and doesn't have default value.");
                }).ToArray();
            }
        }

        internal static void ProcessMethod(ICommand processor, MethodInfo methodInfo, object[] methodParams)
        {
            if (methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null)
            {
                var task = (Task)methodInfo.Invoke(processor, methodParams);
                task.Wait();
            }
            else
            {
                methodInfo.Invoke(processor, methodParams);
            }
        }
    }
}
