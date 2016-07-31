using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Threading;
using CommandLine;
using PoGoBot.Logic;
using PoGoBot.Logic.Automation;

namespace PoGoBot.Console
{
    /*
     * TODO List
     * Eggs (Incubate, Hatching etc.)
     * Item Usage (Lucky Egg, Incense)
     * Sanity checks for settings
     * Proper exceptions
     * Individual Power calculations as alternative for Combat Power
     * Navigation (Simple/Greedy, GPX, TSP)
     * Settings for delay and randomization, simulate gps inaccuracy
     * Refactor that ugly statistic code
     * Statistic - Meters walked
     */

    public class Program
    {
        private static Bot _bot;
        private static Settings _settings;
        private static ResourceManager _rm;
        private static DateTime _lastThink = DateTime.MinValue;
        private static Accounts _accounts;

        private static void Main(string[] args)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            _rm = new ResourceManager("PoGoBot.Console.Resources.Languages.Console.messages",
                Assembly.GetExecutingAssembly());
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var title = $"PoGoBot v{version.Major}.{version.Minor}.{version.Build}";
            System.Console.Title = title;
            Colorful.Console.WriteAscii(title);

            var options = new Options();
            var accountIndex = 0;
            if (Parser.Default.ParseArguments(args, options))
            {
                accountIndex = options.AccountIndex;
            }
            if (GenerateConfigs())
            {
                Output("Bot_Identifier", "Bot_Exit_Now");
                System.Console.ReadKey();
                return;
            }

            Output("Bot_Identifier", "Bot_Load_Settings_Success");
            Output("Bot_Identifier", "Bot_Load_Accounts_Success");
            var account = _accounts.Entries[Math.Min(_accounts.Entries.Count - 1, accountIndex)];
            _bot = new Bot(_settings, account);
            _bot.Authenticated += OnBotAuthenticated;
            _bot.Terminated += OnBotTerminated;
            _bot.Started += OnBotStarted;
            var bListener = new BotEventListener(_bot, 0.25f);
            var statistic = new Statistic(_bot, 5000);
            statistic.Start();
            bListener.StartListen();
            _bot.Start();
            do
            {
                while (!System.Console.KeyAvailable)
                {
                    var difference = Math.Max(0, _settings.Bot.IntervalMilliseconds) -
                                     Math.Abs((DateTime.UtcNow - _lastThink).TotalMilliseconds);
                    if (difference <= 0)
                    {
                        _bot.Execute();
                        _lastThink = DateTime.UtcNow;
                        while (bListener.Messages.Count > 0)
                        {
                            var message = bListener.Messages.Dequeue();
                            Output(message.Message, message.HighlightColor, message.DefaultColor, message.Args);
                        }
                        var runtime = DateTime.UtcNow - statistic.StartTime;
                        var runtimeStr = $"{(int) Math.Floor(runtime.TotalHours)}:{runtime.Minutes}:{runtime.Seconds}";
                        System.Console.Title = $"{account.Username} - {runtimeStr} | {statistic}";
                    }
                    else
                    {
                        Thread.Sleep((int) ++difference);
                    }
                }
            } while (System.Console.ReadKey(true).Key != ConsoleKey.Escape);
            _bot.Terminate();
            bListener.StopListen();
            statistic.Stop();
            Output("Bot_Identifier", "Bot_Exit_Now");
            System.Console.ReadKey();
        }

        private static void OnBotStarted(object sender, EventArgs e)
        {
            Output("Bot_Identifier", "Bot_Started");
            Output($"{_rm.GetString("Bot_Identifier"),10} | {_rm.GetString("Bot_Exit_Info")}",
                Color.Orange, Color.White, "ESC");
        }

        private static void OnBotTerminated(object sender, EventArgs e)
        {
            Output("Bot_Identifier", "Bot_Terminated");
        }

        private static void OnBotAuthenticated(object sender, EventArgs e)
        {
            Output("Bot_Identifier", "Bot_Authenticated");
        }

        private static bool GenerateConfigs()
        {
            bool newConfig;
            _settings = Settings.Generate(out newConfig);
            if (_settings == null || newConfig)
            {
                Output("Bot_Identifier", $"Load_Settings_{(newConfig ? "New" : "Error")}");
            }

            bool newAccounts;
            _accounts = Accounts.Generate(out newAccounts);
            if (_accounts == null || newAccounts)
            {
                Output("Bot_Identifier", $"Load_Accounts_{(newConfig ? "New" : "Error")}");
            }
            return newConfig || newAccounts;
        }

        private static void Output(string message, Color highlightColor, Color defaultColor, params object[] args)
        {
            Colorful.Console.WriteLineFormatted($"[{DateTime.Now.ToString("HH:mm:ss")}]: {message}", highlightColor,
                defaultColor, args);
        }

        private static void Output(string identifier, string message)
        {
            identifier = _rm.GetString(identifier);
            message = _rm.GetString(message);
            Colorful.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}]: {identifier,10} | {message}");
        }
    }
}