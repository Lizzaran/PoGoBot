using System;
using PoGoBot.Logic.Interfaces;
using POGOLib.Net;

namespace PoGoBot.Logic.Automation.Filters
{
    public abstract class BaseFilter<T> : IFilter<T>
    {
        private readonly Func<bool> _enabledFunction;

        protected BaseFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
        {
            Settings = settings;
            Session = session;
            _enabledFunction = enabledFunction;
        }

        protected Settings Settings { get; }
        protected Session Session { get; }

        public bool Enabled
        {
            get
            {
                if (_enabledFunction != null)
                {
                    return _enabledFunction();
                }
                return true;
            }
        }

        public abstract T Process(T input);
    }
}