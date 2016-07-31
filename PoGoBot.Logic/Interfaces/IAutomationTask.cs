namespace PoGoBot.Logic.Interfaces
{
    public interface IAutomationTask
    {
        bool Enabled { get; }
        bool ShouldExecute { get; }
        void OnStart();
        void OnExecute();
        void OnTerminate();
    }
}