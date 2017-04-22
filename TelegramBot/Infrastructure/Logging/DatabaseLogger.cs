using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate;
using TelegramBot.Logging;
using TelegramBot.Persistence;

namespace TelegramBot.Infrastructure.Logging
{
    public class LogDTO
    {
        public virtual int Id { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual int Level { get; set; }
        public virtual string Message { get; set; }


        public class Mappings : ClassMap<LogDTO>
        {
            public Mappings()
            {
                Table("TelegramLog");
                Id(t => t.Id).GeneratedBy.Increment();
                Map(t => t.DateTime);
                Map(t => t.Level);
                Map(t => t.Message);
                
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}; DateTime: {DateTime}; Level: {Level}; Message: {Message}";
        }
    }

    public class DatabaseLogger : ILogger
    {
        private readonly IPersistanceManager _persistence;

        public DatabaseLogger(IPersistanceManager persistence)
        {
            _persistence = persistence;
        }

        public void Log(LogLevel level, object item)
        {
            var dto = new LogDTO
            {
                DateTime = DateTime.UtcNow,
                Level = (int) level,
                Message = item.ToString()
            };

            _persistence.Save(dto);
        }
    }
}
