using GeoCoordinatePortable;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Helpers;
using POGOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Tasks.Player
{
    public class FollowRouteTask : BaseTask
    {
        private int _previousPoint;
        private int _nextPoint;
        private long _time;

        public FollowRouteTask(Context context) : base(context)
        {
        }

        public override bool Enabled => Context.Settings.Bot.FollowRoute.Enabled;

        public override void OnExecute()
        {
            var time = TimeUtil.GetCurrentTimestampInMilliseconds();
            if (_time + Context.Settings.Bot.FollowRoute.Speed * 1000 < time)
            {
                var nextStep = GetNextStep();
                var playerUpdateResponse = Context.RpcRequest.Player.Update(nextStep.Latitude, nextStep.Longitude);
                Context.Events.DispatchEvent(this, new PlayerUpdateEventArgs(playerUpdateResponse, nextStep.Latitude, nextStep.Longitude));
                _time = time;
            }
        }

        public override void OnStart()
        {
            _previousPoint = 0;
            _nextPoint = 0;
            FindNextPoint();
        }

        public override void OnTerminate()
        {

        }

        private void FindNextPoint()
        {
            var nextPoints = Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].RouteLinks.Where(c => c != _previousPoint).ToList();
            if (nextPoints.Count() == 0)
            {
                int tmpPoint = _previousPoint;
                _previousPoint = _nextPoint;
                _nextPoint = tmpPoint;
            }
            else
            {
                _previousPoint = _nextPoint;
                if (nextPoints.Count() == 1)
                {
                    _nextPoint = nextPoints.ElementAt(0);
                }
                else
                {
                    Random rnd = new Random();
                    _nextPoint = nextPoints.ElementAt(rnd.Next(nextPoints.Count()));
                }
            }
            Context.Events.DispatchEvent(this, new RouteNextPointEventArgs(_nextPoint, 
                Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].Position.Latitude,
                Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].Position.Longitude));
        }

        private GeoCoordinate GetNextStep()
        {
            var distance = Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].Position.GetDistanceTo(new GeoCoordinate(Context.Session.Player.Latitude, Context.Session.Player.Longitude));
            if (distance < Context.Settings.Bot.FollowRoute.StepSize)
            {
                var nextStep = Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].Position;
                FindNextPoint();
                return nextStep;
            }
            var latitude = Context.Session.Player.Latitude + Math.Round((Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].Position.Latitude - Context.Session.Player.Latitude) *
                Context.Settings.Bot.FollowRoute.StepSize / distance, 6);
            var longitude = Context.Session.Player.Longitude + Math.Round((Context.Settings.Bot.FollowRoute.RoutePoints[_nextPoint].Position.Longitude - Context.Session.Player.Longitude) *
                Context.Settings.Bot.FollowRoute.StepSize / distance, 6);
            return new GeoCoordinate(Utils.RandomizeCoordinate(latitude), Utils.RandomizeCoordinate(longitude));
        }
    }
}
