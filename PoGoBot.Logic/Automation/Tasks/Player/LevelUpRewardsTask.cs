using PoGoBot.Logic.Automation.Events.Tasks.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Tasks.Player
{
    public class LevelUpRewardsTask : BaseTask
    {

        private int CurrentLevel;

        public LevelUpRewardsTask(Context context) : base(context)
        {
        }

        public override bool Enabled => Context.Settings.Bot.LevelUpRewards.Enabled;

        public override void OnExecute()
        {
            while (CurrentLevel < Context.Session.Player.Stats.Level)
            {
                CurrentLevel ++;

                var levelUpRewardsResponse = Context.RpcRequest.Player.LevelUpRewards(CurrentLevel);
                Context.Events.DispatchEvent(this, new LevelUpRewardsEventArgs(levelUpRewardsResponse, CurrentLevel));
            }
        }

        public override void OnStart()
        {
            CurrentLevel = Context.Session.Player.Stats.Level;
        }

        public override void OnTerminate()
        {
            
        }
        
    }
}
