using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using TelegramBot.API.Models;
using TelegramBot.Bot;
using TelegramBot.Logging;
using TelegramBot.WebHost.Telegram;

namespace TelegramBot.WebHost.Controllers
{
    public class WebHookController : ApiController
    {
        [Inject]
        public ILogger Logger { get; set; }

        private readonly IWebHookBot _bot;

        public WebHookController(IWebHookBot bot)
        {
            _bot = bot;
        }

        [HttpPost]
        [Route("webhook/{token}")]
        public async Task<IHttpActionResult> GetUpdates(string token, [FromBody] Update update)
        {
            Logger?.Log(LogLevel.Message, $"Received request on token {token} with update: {update?.Message?.Text}");
            if (!string.Equals(token, TelegramWebHook.Token, StringComparison.OrdinalIgnoreCase)) return BadRequest();

            await _bot.ProcessUpdate(update);

            return Ok();
        }
    }
}
