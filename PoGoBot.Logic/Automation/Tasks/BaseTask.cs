using PoGoBot.Logic.Interfaces;

namespace PoGoBot.Logic.Automation.Tasks
{
    public abstract class BaseTask : IAutomationTask
    {
        protected BaseTask(Context context)
        {
            Context = context;
        }

        public Context Context { get; }
        public abstract bool Enabled { get; }

        public virtual bool ShouldExecute
            =>
                !Context.Traits.IsSoftbanned && !Context.Traits.IsEncountering &&
                Context.Session.Player?.Inventory != null &&
                Context.Session.Map?.Cells != null;

        public abstract void OnStart();
        public abstract void OnExecute();
        public abstract void OnTerminate();
    }
}