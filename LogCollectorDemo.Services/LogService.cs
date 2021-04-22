using LogCollectorDemo.Core.Repositories;
using LogCollectorDemo.Core.Entities;
using System;
using System.Collections.Generic;
using LogCollectorDemo.Core.Services;
using Microsoft.Extensions.Logging;
using LogCollectorDemo.Core.Models;

namespace LogCollectorDemo.Services
{
    public class LogService : ILogService
    {
        public ILogParser LogsParser { get; }
        private readonly IRepository<EventEntity> _repo;
        private readonly ILogger<LogService> _logger;

        public LogService(IRepository<EventEntity> repo, ILogger<LogService> logger, ILogParser logParser)
        {
            _repo = repo;
            _logger = logger;
            LogsParser = logParser;
        }

        public ParsingResult ProcessLogFile(string path)
        {
            LogsParser.LogFilePath = path;
            return LogsParser.Parse();
        }
    }

    public class EventService : IEventService
    {
        private readonly IRepository<EventEntity> _repo;
        private readonly ILogger<EventService> _logger;

        public EventService(IRepository<EventEntity> repo, ILogger<EventService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public void SaveEvent(Log log1, Log log2)
        {
            int duration = (int)Math.Abs(log1.Timestamp - log2.Timestamp);
            EventEntity entity = new EventEntity(log1.Id, duration, log1.Type, log1.Host, duration > 4);
            _repo.Insert(entity);
        }

        public List<EventEntity> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
