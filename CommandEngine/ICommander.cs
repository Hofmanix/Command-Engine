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
        void UpdateLast(CommandText message);
        void UpdateLast(Func<CommandText, CommandText> messageBuilder);
        void Clear();
        void OnGameCreated(IGame game);
    }

    public static class ICommanderExtensions
    {
        public static void Write(this ICommander commander, string message)
            => commander.Write(new CommandText(message));

        public static void UpdateLast(this ICommander commander, string message)
            => commander.UpdateLast(new CommandText(message));

        public static void UpdateLast(this ICommander commander, Func<string, string> messageBuilder)
            => commander.UpdateLast(message =>
            {
                message.Text = messageBuilder(message.Text);
                return message;
            });
    }
}