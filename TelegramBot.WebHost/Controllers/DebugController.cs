using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TelegramBot.Infrastructure.Logging;
using TelegramBot.Persistence;

namespace TelegramBot.WebHost.Controllers
{
    [RoutePrefix("Debug")]
    public class DebugController : ApiController
    {
        private readonly IRepository<LogDTO> _logs;

        public DebugController(IRepository<LogDTO> logs)
        {
            _logs = logs;
        }

        [HttpGet()]
        [Route()]
        public IHttpActionResult Get(int count = 30)
        {
            return Ok(string.Join("\r\n", _logs.GetAll().OrderByDescending(t=>t.DateTime).Take(count).ToList()));
        }
    }
}
