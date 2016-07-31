using System.Drawing;

namespace PoGoBot.Console
{
    internal class EventMessage
    {
        public EventMessage(string message, Color highlightColor, Color defaultColor, params object[] args)
        {
            Message = message;
            HighlightColor = highlightColor;
            DefaultColor = defaultColor;
            Args = args;
        }

        public string Message { get; set; }
        public Color HighlightColor { get; set; }
        public Color DefaultColor { get; set; }
        public object[] Args { get; set; }
    }
}