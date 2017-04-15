namespace TelegramBot.Bot.Replies
{
    public class ImageReply : IReply
    {
        public byte[] Image { get; }
        public string Caption { get; }
        public long ChatId { get; set; }


        public ImageReply(long chatId, byte[] image, string caption = null)
        {
            ChatId = chatId;
            Image = image;
            Caption = caption;
        }

        public TResult AcceptVisitor<TArgs, TResult>(IReplyVisitor<TArgs, TResult> visitor, TArgs args)
        {
            return visitor.VisitImage(this, args);
        }

        
        public override string ToString()
        {
            return Caption;
        }
    }
}
