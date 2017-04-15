namespace TelegramBot.Bot.Replies
{
    public class TextReply : IReply
    {
        public string Text { get; set; }

        public TextReply(long chatId, string text)
        {
            ChatId = chatId;
            Text = text;
        }

        public TResult AcceptVisitor<TArgs, TResult>(IReplyVisitor<TArgs, TResult> visitor, TArgs args)
        {
            return visitor.VisitText(this, args);
        }

        public long ChatId { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
