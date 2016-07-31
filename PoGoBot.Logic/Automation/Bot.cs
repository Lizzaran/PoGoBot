using System;
using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Api;
using PoGoBot.Logic.Automation.Events.Tasks;
using PoGoBot.Logic.Automation.Tasks;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;

namespace PoGoBot.Logic.Automation
{
    public class Bot : BaseBot
    {
        public Bot(Settings settings, Account account) : base(settings, account)
        {
            Events = new EventDispatcher();
            Started += OnStarted;
        }

        public EventDispatcher Events { get; set; }

        public List<IAutomationTask> Tasks { get; private set; } = new List<IAutomationTask>();

        private void OnStarted(object sender, EventArgs eventArgs)
        {
            var context = new Context(Settings, Session, new Traits(Events), new RpcRequest(Session), Events);
            Tasks = Utils.CreateInstancesOf<IAutomationTask>(null, context).Where(t => t.Enabled).ToList();
            foreach (var task in Tasks)
            {
                task.OnStart();
            }
        }

        public new void Terminate()
        {
            foreach (var task in Tasks)
            {
                task.OnTerminate();
            }
            base.Terminate();
        }

        public void Execute()
        {
            foreach (var task in Tasks.Where(t => t.ShouldExecute))
            {
                task.OnExecute();
            }
        }
    }
}