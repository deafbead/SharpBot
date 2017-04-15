using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Replies
{
    public class DocumentReply : IReply
    {
        public byte[] Document { get; }
        public string Caption { get; }

        public DocumentReply(long chatId, byte[] document, string caption = null)
        {
            ChatId = chatId;
            Document = document;
            Caption = caption;
        }

        public TResult AcceptVisitor<TArgs, TResult>(IReplyVisitor<TArgs, TResult> visitor, TArgs args)
        {
            return visitor.VisitDocument(this, args);
        }

        public long ChatId { get; set; }

        public override string ToString()
        {
            return Caption;
        }
    }
}
