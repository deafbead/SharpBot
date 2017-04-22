using Newtonsoft.Json;

namespace TelegramBot.API.Models
{
    /// <summary>
    /// This object represents an incoming callback query from a callback button in an inline keyboard. 
    /// <see>
    /// https://core.telegram.org/bots/api#callbackquery
    /// </see>
    /// </summary>
    public class CallbackQuery
    {
        /// <summary>
        /// Unique identifier for this query
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Sender
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Optional. Message with the callback button that originated the query. 
        /// Note that message content and message date will not be available if the message is too old
        /// </summary>
        [JsonProperty("message")]
        public Message Message { get; set; }

        /// <summary>
        /// Optional. Identifier of the message sent via the bot in inline mode, that originated the query.
        /// </summary>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Global identifier, uniquely corresponding to the chat to which the message with the callback button was sent. 
        /// Useful for high scores in games.
        /// </summary>
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }

        /// <summary>
        /// Optional. Data associated with the callback button. Be aware that a bad client can send arbitrary data in this field.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Optional. Short name of a Game to be returned, serves as the unique identifier for the game
        /// </summary>
        [JsonProperty("game_short_name")]
        public string GameShortName { get; set; }
    }
}
