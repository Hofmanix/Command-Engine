using Microsoft.Extensions.DependencyInjection;

namespace CommandEngine
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureCommands(IGameBuilder gameBuilder);
        void ConfigureGame(IGame game);
    }
}