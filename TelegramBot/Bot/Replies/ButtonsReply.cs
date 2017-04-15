using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.API.Models;

namespace TelegramBot.Bot.Replies
{
    public class ButtonsReply : IReply
    {
        public string Title { get; }
        public ReplyKeyboardMarkup Markup { get; set; }

        public ButtonsReply(long chatId, string title, KeyboardButton[][] buttons)
        {
            ChatId = chatId;
            Title = title;
            Markup = new ReplyKeyboardMarkup()
            {
                Keyboard = buttons
            };
        }

        public TResult AcceptVisitor<TArgs, TResult>(IReplyVisitor<TArgs, TResult> visitor, TArgs args)
        {
            return visitor.VisitButtons(this, args);
        }

        public long ChatId { get; set; }
    }
}
