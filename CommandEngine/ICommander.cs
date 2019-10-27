using System;
using System.Threading.Tasks;

namespace CommandEngine
{
    public interface ICommander
    {
        Task<string> ReadCommand();
        void Write(string message);
        void WriteLine(string message);
        void UpdateLine(int lineIndex, string message);
        void UpdateLine(int lineIndex, Func<string, string> messageBuilder);
        void Clear();
    }
}