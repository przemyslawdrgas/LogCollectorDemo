using LogCollectorDemo.Core.Models;
using LogCollectorDemo.Core.Services;
using LogCollectorDemo.Resources;
using LogCollectorDemo.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace LogCollectorDemo
{
    public class App
    {
        private readonly ILogService _logService;
        private readonly IEventService _eventService;
        private readonly ILogger<App> _logger;

        public App(ILogService logService, IEventService eventService, ILogger<App> logger)
        {
            _logService = logService;
            _eventService = eventService;
            _logger = logger;
        }

        public void Run()
        {
            Start();

            try
            {
                string path = GetLogFilePath();
                ParsingResult result = _logService.ProcessLogFile(path);
                Console.WriteLine();
                ReportResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                _logger.LogDebug(Interface.ProcFinished);
            }
        }

        private string GetLogFilePath()
        {
            _logger.LogInformation($"{Interface.GivePathInfo}");
            var path = Console.ReadLine();
            _logger.LogDebug($"{Interface.Processing}... ({path}){Environment.NewLine}");
            return path;
        }

        private void Start()
        {
            _logger.LogDebug(Interface.AppStart);
            AddHandlers();
        }

        private void AddHandlers()
        {
            _logService.LogsParser.LineProccesed += new EventHandler(ShowProgress);
            _logService.LogsParser.MatchFound += new EventHandler<MatchEventArgs>(HandleMatch);
        }

        private void ReportResult(ParsingResult result)
        {
            if (result.ExceptionsCount > 0)
                _logger.LogWarning(string.Format(Interface.ExceptionsInfo, result.ExceptionsCount, result.DebugLogPath));

            if (result.UnmatchedLogs.Any())
            {
                _logger.LogInformation($"{Environment.NewLine}{Interface.MissingMatchInfo}:");
                Console.WriteLine($"{Interface.Id.ToUpper(),-20} {Interface.State.ToUpper(),-20}");

                foreach (Log log in result.UnmatchedLogs)
                {
                    string message = $"{log.Id,-20} {log.State,-20}";
                    Console.WriteLine(message);
                }

                Console.WriteLine();
            }
        }

        private void ShowProgress(object s, EventArgs e)
        {
            var parser = (LogParser)s;
            var progress = parser.Progress.ToString("P2");
            _logger.LogInformation($"{Interface.Progress} : {progress,5} ({parser.linesProcessed}/{parser.LinesCount})");
        }

        private void HandleMatch(object s, MatchEventArgs e)
        {
            _eventService.SaveEvent(e.Log1, e.Log2);
        }
    }
}
