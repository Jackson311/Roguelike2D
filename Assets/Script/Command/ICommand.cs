namespace Script.Command
{
    public interface ICommand
    {
        void DoCommand();

        void UnDoCommand();
    }
}