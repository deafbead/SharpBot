using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ninject;
using Ninject.Infrastructure.Language;
using Ninject.Syntax;
using TelegramBot.API;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands.Attributes;
using TelegramBot.Bot.Replies;

namespace TelegramBot.Bot.Commands
{
    class ReflectionCommandInvoker : ICommandInvoker
    {
        public Type AttributeType { get; }
        public ICollection<Command> Commands { get; }

        public ReflectionCommandInvoker(IResolutionRoot kernel) : this(kernel, null)
        {
        }

        public ReflectionCommandInvoker(IResolutionRoot kernel, Type attributeType)
        {
            AttributeType = attributeType;
            Commands = GetCommands(kernel).ToList();
        }

        public async Task<IEnumerable<IReply>> Invoke(TelegramMessageEventArgs input)
        {
            var result = new List<IReply>();
            foreach (var command in Commands)
            {
                if (!command.ShouldInvoke(input)) continue;
                var output = await command.Invoke(input);
                result.AddRange(output);
            }
            return result;
        }

        private IEnumerable<Command> GetCommands(IResolutionRoot kernel)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(ShouldInvoke);

            foreach (var commandType in types)
            {
                yield return (Command)kernel.Get(commandType);
            }
        }

        private bool ShouldInvoke(Type command)
        {
            if (!typeof(Command).IsAssignableFrom(command)) return false;
            if (command.IsAbstract) return false;

            if ((AttributeType == null) && !HasCommandAttributes(command)) return true;

            return AttributeType != null && command.HasAttribute(AttributeType);
        }

        private bool HasCommandAttributes(Type type)
        {
            return type.CustomAttributes.Any(ca => typeof(CommandAttribute).IsAssignableFrom(ca.AttributeType));
        }
    }
}
