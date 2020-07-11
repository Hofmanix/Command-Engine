using CommandEngine.Attributes;
using CommandEngine.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CommandEngine
{
    public abstract class Command : ICommand
    {
        protected internal ICommander Commander { get; internal set; }
        protected internal IGame Game { get; internal set; }
        protected internal IArea Area { get; internal set; }
        protected event Action<Exception> NamedFunctionMappingFailed;

        public void ProcessByGame(string[] parameters)
        {
            if ((parameters?.Any() ?? false) && ProcessNamedFunctionByParameters(parameters))
            {
                return;
            }

            var processMethod = GetType().GetMethods()
                .FirstOrDefault(method => method.Name == "Process" && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(string[]));
            if (processMethod != null)
            {
                CommandProcessor.ProcessMethod(this, processMethod, new object[] { parameters });
            }
        }

        private bool ProcessNamedFunctionByParameters(string[] parameters)
        {
            try
            {
                var namedMember = CommandProcessor.GetNamedMember(this, parameters.First());
                if (namedMember != null)
                {
                    if (namedMember is PropertyInfo namedProperty)
                    {
                        Commander.Write(namedProperty.GetValue(this).ToString());
                    }
                    else
                    {
                        var namedMethod = namedMember as MethodInfo;
                        var methodParams = CommandProcessor.ConvertMethodParameters(namedMethod, parameters.Where((param, index) => index != 0).ToArray(), Game.GameOptions.MethodParametersNaming);
                        CommandProcessor.ProcessMethod(this, namedMethod, methodParams);
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                NamedFunctionMappingFailed?.Invoke(ex);
            }

            return false;
        }
    }
}