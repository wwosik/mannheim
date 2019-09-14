namespace Mannheim.Cli
{
    public interface ICommandFactory
    {
        string AvailableCommandsText { get; }

        CommandInfo FindCommand(string name);
    }
}