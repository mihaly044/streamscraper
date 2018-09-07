using System;
using System.Collections.Generic;
using System.Text;

namespace streamscraper
{
    public static class ConsoleKit
    {
        public enum MessageType
        {
            INFO,
            WARNING,
            ERROR,
            DEBUG,
            INPUT
        }

        public static void Message(MessageType type, string message, params object[] args)
        {
            var color = ConsoleColor.Black;
            switch (type)
            {
                case MessageType.INFO:
                    color = ConsoleColor.Cyan;
                    break;
                case MessageType.WARNING:
                    color = ConsoleColor.Yellow;
                    break;
                case MessageType.ERROR:
                    color = ConsoleColor.Red;
                    break;
                case MessageType.DEBUG:
                    color = ConsoleColor.DarkCyan;
                    break;
                case MessageType.INPUT:
                    color = ConsoleColor.Magenta;
                    break;
                default:
                    throw new ArgumentException("Invalid message type specified");
            }
            Console.ForegroundColor = color;

            Console.Write("[{0}]", Enum.GetName(typeof(MessageType), type));
            Console.ResetColor();
            Console.Write(" {0}", string.Format(message, args));
        }
    }
}
