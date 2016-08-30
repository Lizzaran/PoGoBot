using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Tasks.Item
{
    public class EggIncubatorTask : BaseTask
    {

        public EggIncubatorTask(Context context) : base(context)
        {
        }

        public override bool Enabled => Context.Settings.Bot.EggIncubator.Enabled;

        public override void OnExecute()
        {
            var eggIncubatorsGlobal = Context.Session.Player.Inventory.GetEggIncubators();
            foreach (var eggIncubators in eggIncubatorsGlobal)
            {
                foreach (var eggIncubator in eggIncubators.EggIncubator.Where(e => e.PokemonId == 0))
                {
                    var egg = Context.Session.Player.Inventory.GetEggs().Where(e => e.EggIncubatorId.Length == 0).First();

                    var useItemEggIncubatorResponse = Context.RpcRequest.Item.UseEggIncubator(eggIncubator.Id, egg.Id);
                    Context.Events.DispatchEvent(this, new UseItemEggIncubatorArgs(useItemEggIncubatorResponse, eggIncubator.Id, egg.Id));
                }

            }
        }

        public override void OnStart()
        {
            
        }

        public override void OnTerminate()
        {

        }

    }
}
