namespace CommandEngine
{
    public interface ICommand
    {
        /// <summary>
        /// Method called by game when parameter comes
        /// </summary>
        /// <param name="parameters"></param>
        void ProcessByGame(string[] parameters);
    }
}