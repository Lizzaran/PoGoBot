using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using POGOLib.Net;
using POGOLib.Net.Authentication;
using POGOLib.Net.Authentication.Data;
using POGOLib.Pokemon.Data;

namespace PoGoBot.Logic.Automation
{
    public abstract class BaseBot : IDisposable
    {
        protected BaseBot(Settings settings, Account account)
        {
            Settings = settings;
            Account = account;
        }

        public Settings Settings { get; }
        public Account Account { get; }
        public Session Session { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event EventHandler<EventArgs> Started;
        public event EventHandler<EventArgs> Authenticated;
        public event EventHandler<EventArgs> Terminated;

        public void Terminate()
        {
            Session?.Shutdown();
            Terminated?.Invoke(this, EventArgs.Empty);
        }

        protected void Authenticate()
        {
            Session = GetSession(
                Account.Username,
                Account.Password,
                Account.Provider,
                Account.Position.Latitude,
                Account.Position.Longitude
                );
            Session.AccessTokenUpdated += OnSessionAccessTokenUpdated;
            Authenticated?.Invoke(this, EventArgs.Empty);
        }

        public bool Start()
        {
            Authenticate();
            var started = Session.Startup();
            if (started)
            {
                Started?.Invoke(this, EventArgs.Empty);
            }
            return started;
        }

        private void OnSessionAccessTokenUpdated(object sender, EventArgs eventArgs)
        {
            SaveAccessToken(Session.AccessToken);
        }

        private Session GetSession(string username, string password, LoginProvider loginProvider, double initLat,
            double initLong)
        {
            var file = Path.Combine(Environment.CurrentDirectory,
                ConfigurationManager.AppSettings["PoGoBot.Logic.Tokens.Directory"] ?? string.Empty,
                $"{username}-{loginProvider}.json");
            if (File.Exists(file))
            {
                var accessToken = JsonConvert.DeserializeObject<AccessToken>(File.ReadAllText(file));
                if (!accessToken.IsExpired)
                {
                    return Login.GetSession(accessToken, password, initLat, initLong);
                }
            }
            var session = Login.GetSession(username, password, loginProvider, initLat, initLong);
            SaveAccessToken(session.AccessToken);
            return session;
        }

        private void SaveAccessToken(AccessToken accessToken)
        {
            var file = Path.Combine(Environment.CurrentDirectory,
                ConfigurationManager.AppSettings["PoGoBot.Logic.Tokens.Directory"] ?? string.Empty,
                $"{accessToken.Uid}.json");
            var directory = Path.GetDirectoryName(file);
            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            File.WriteAllText(file, JsonConvert.SerializeObject(accessToken, Formatting.Indented));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Session?.Dispose();
            }
        }
    }
}