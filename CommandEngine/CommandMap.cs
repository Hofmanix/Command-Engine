using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandEngine
{
    internal class CommandMap
    {
        public Type this[string command]
        {
            get
            {
                return _map[command.Split(' ').First().ToLower()];
            }
        }

        public IReadOnlyCollection<Type> CommandTypes => _map.Values;

        private Dictionary<string, Type> _map;

        public CommandMap()
        {
            _map = new Dictionary<string, Type>();
        }

        public void Add(string key, Type commandType)
        {
            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new ArgumentException("Given type is not assignable from ICommand.");
            }

            if (key.Split(' ').Length > 1)
            {
                throw new ArgumentException("Command key can contain only one word.");
            }

            if (_map.ContainsKey(key))
            {
                throw new ArgumentException("Command with given name already exists.");
            }
            _map.Add(key, commandType);
        }
    }
}
