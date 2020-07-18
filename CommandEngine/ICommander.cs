using System;
using System.Threading.Tasks;
using System.Xml;

namespace CommandEngine
{
    public interface ICommander
    {
        string Location { get; set; }
        Task<string> ReadCommand();
        void Write(CommandText message);
        void Update(int messageIndex, CommandText message);
        void Update(int messageIndex, Func<CommandText, CommandText> messageBuilder);
        void Clear();
        void OnGameCreated(IGame game);
    }

    public static class ICommanderExtensions
    {
        public static void Write(this ICommander commander, string message)
            => commander.Write(new CommandText(message));

        public static void Update(this ICommander commander, int messageIndex, string message)
            => commander.Update(messageIndex, new CommandText(message));

        public static void Update(this ICommander commander, int messageIndex, Func<string, string> messageBuilder)
            => commander.Update(messageIndex, message =>
            {
                message.Text = messageBuilder(message.Text);
                return message;
            });
    }
}