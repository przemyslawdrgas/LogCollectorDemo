using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using LogCollectorDemo.Core.Services;
using Microsoft.Extensions.Logging;
using LogCollectorDemo.Core.Models;

namespace LogCollectorDemo.Services
{
    public class LogParser : ILogParser
    {
        private readonly ILogger<LogParser> _logger;

        public LogParser(ILogger<LogParser> logger)
        {
            _logger = logger;
        }

        public string LogFilePath { get; set; }
        public string DebugLogPath { get; private set; }
        public List<Log> LogsToProcess { get; private set; } = new List<Log>();
        public int linesProcessed { get; private set; }
        public int LinesCount { get; private set; }
        public double Progress => (float)linesProcessed / LinesCount;

        public ParsingResult Parse()
        {
            int exceptionsCount = 0;
            int currLineIndex = -1;

            try
            {
                using (FileStream fs = File.OpenRead(LogFilePath))
                using (StreamReader sr = new StreamReader(fs))
                {
                    GetLinesCount();

                    using (FileStream dlfs = File.Create(SetProcLogPath()))
                    using (StreamWriter dlsw = new StreamWriter(dlfs))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            currLineIndex++;
                            OnLineProccesed();

                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            try
                            {
                                HandleLine(line);
                            }
                            catch (Exception e)
                            {
                                exceptionsCount++;
                                string message = $"File line {currLineIndex}: {e.Message}";
                                dlsw.WriteLine(message);
                                _logger.LogError(message);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }


            return new ParsingResult(exceptionsCount, LogsToProcess, DebugLogPath);
        }

        private string SetProcLogPath()
        {
            DateTime date = DateTime.Now;
            string dlName = $"log_{date.Year}{date.Month}{date.Day}{date.Hour}{date.Minute}{date.Second}.txt";
            string dlPath = $"{Path.GetDirectoryName(LogFilePath)}\\{dlName}";

            return DebugLogPath = dlPath;
        }

        private void HandleLine(string line)
        {
            Log log = JsonConvert.DeserializeObject<Log>(line);
            Log matchingLog = LogsToProcess.Where(x => x.Id == log.Id).FirstOrDefault();

            if (matchingLog != null)
            {
                LogsToProcess.Remove(matchingLog);
                OnMatchFound(new MatchEventArgs(log, matchingLog));
            }
            else
            {
                LogsToProcess.Add(log);
            }
        }

        private void GetLinesCount()
        {
            using (StreamReader r = new StreamReader(LogFilePath))
            {
                while (r.ReadLine() != null) { LinesCount++; }
            }
        }

        public event EventHandler LineProccesed;
        public void OnLineProccesed()
        {
            linesProcessed++;
            LineProccesed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<MatchEventArgs> MatchFound;
        public void OnMatchFound(MatchEventArgs matchEventArgs)
        {
            MatchFound?.Invoke(this, matchEventArgs);
        }
    }
}
