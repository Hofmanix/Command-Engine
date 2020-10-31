using System.Threading.Tasks;
using CommandEngine;

namespace CommandEngine.Controls
{
    public class CommanderEvent
    {
        public CommanderEventType Type { get; }
        private TaskCompletionSource<string> _readLineCompletitionSource;
        private TaskCompletionSource<KeyInfo> _readKeyCompletitionSource;

        public CommanderEvent(TaskCompletionSource<string> readLineCompletitionSource)
        {
            Type = CommanderEventType.ReadLine;
            _readLineCompletitionSource = readLineCompletitionSource;
        }

        public CommanderEvent(TaskCompletionSource<KeyInfo> readKeyCompletitionSource)
        {
            Type = CommanderEventType.ReadKey;
            _readKeyCompletitionSource = readKeyCompletitionSource;
        }

        public void SetResult(string message)
        {
            _readLineCompletitionSource.SetResult(message);
        }

        public void SetResult(KeyInfo keyInfo)
        {
            _readKeyCompletitionSource.SetResult(keyInfo);
        }
    }

    public enum CommanderEventType
    {
        ReadKey,
        ReadLine
    }
}