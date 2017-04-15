using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    abstract class CommandAttribute : Attribute
    {
    }
}
