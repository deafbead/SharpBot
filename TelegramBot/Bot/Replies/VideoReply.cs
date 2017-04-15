using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Bot.Replies
{
    public class VideoReply : IReply
    {
        public VideoReply(long chatId, byte[] video, string caption = null)
        {
            ChatId = chatId;
            Video = video;
            Caption = caption;
        }

        public byte[] Video { get; }
        public string Caption { get; }

        public TResult AcceptVisitor<TArgs, TResult>(IReplyVisitor<TArgs, TResult> visitor, TArgs args)
        {
            return visitor.VisitVideo(this, args);
        }

        public long ChatId { get; set; }

        public override string ToString()
        {
            return Caption;
        }
    }
}
