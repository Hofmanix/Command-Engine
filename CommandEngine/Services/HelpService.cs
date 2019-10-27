using CommandEngine.Attributes;
using CommandEngine.Commands;
using CommandEngine.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommandEngine.Services
{
    public class HelpService: IHelpService
    {
        private readonly IGame _game;
        private readonly HelpServiceOptions _options;
        private Dictionary<MemberInfo, string> _memberHelpTexts;
        private Dictionary<Type, MemberInfo[]> _commandMembers;
        private Dictionary<Type, string> _commandHelpTexts;

        public HelpService(IGame game, HelpServiceOptions options)
        {
            _game = game;
            _options = options;
        }

        public void LoadHelpTexts()
        {
            _commandHelpTexts = _game.AllCommands.ToDictionary(command => command, command => command.IsDefined(typeof(HelpAttribute)) ? command.GetCustomAttribute<HelpAttribute>().Description : null);
            _commandMembers = _game.AllCommands.ToDictionary(command => command, command => command.GetMembers().Where(member => member.IsDefined(typeof(NameAttribute))).ToArray());
            _memberHelpTexts= _commandMembers.Values.SelectMany(val => val)
                .ToDictionary(member => member, member => member.IsDefined(typeof(HelpAttribute)) ? member.GetCustomAttribute<HelpAttribute>().Description : null);
        }

        public string GetHelpTextForArea(IArea area)
        {
            return _options.GlobalCommandsTitle + ":\n" +
                string.Join("\n", _game.GlobalCommands.Select(cmd => GetCommandHelp(cmd))) + "\n\n" +
                _options.AreaCommandsTitle + ":\n" +
                string.Join("\n", area.AvailableCommands.Select(cmd => GetCommandHelp(cmd)));
        }

        public string GetCommandHelp(ICommand command)
        {
            return GetCommandHelp(command.GetType());
        }

        private string GetCommandHelp(Type commandType)
        {
            return $"    {commandType.GetCommandName()}:" + (_commandHelpTexts[commandType] != null ? $" {_commandHelpTexts[commandType]}" : "") + "\n"
                + string.Join("\n", _commandMembers[commandType].Select(member => $"        {GetCommandMemberHelp(member)}"));
        }

        private string GetCommandMemberHelp(MemberInfo member)
        {
            return member.GetCustomAttribute<NameAttribute>().Name + ": " + (_memberHelpTexts[member] != null ? $" {_memberHelpTexts[member]}" : "");
        }
    }

    public class HelpServiceOptions
    {
        public string GlobalCommandsTitle { get; set; } = "Global commands";
        public string AreaCommandsTitle { get; set; } = "Commands available in area";
    }

    public static class IGameExtensions
    {
        public static void AddHelp(this IServiceCollection services, Action<HelpServiceOptions> options = null)
        {
            var helpServiceOptions = new HelpServiceOptions();
            options?.Invoke(helpServiceOptions);

            services.AddSingleton<IHelpService>(svc => new HelpService(svc.GetRequiredService<IGame>(), helpServiceOptions));
        }

        public static void AddHelp(this IGameBuilder gameBuilder)
        {
            gameBuilder.AddCommand<HelpCommand>();
        }

        public static void UseHelp(this IGame game)
        {
            game.GameServices.GetService<IHelpService>().LoadHelpTexts();
        }
    }

}
