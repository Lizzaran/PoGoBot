using CommandLine;
using CommandLine.Text;

namespace PoGoBot.Console
{
    internal class Options
    {
        [Option('a', "account", DefaultValue = 0, HelpText = "Selects the x account from the list.")]
        public int AccountIndex { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}