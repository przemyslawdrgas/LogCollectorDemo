using LogCollectorDemo.Core;
using System.Collections.Generic;

namespace LogCollectorDemo.Core.Models
{
    public class ParsingResult
    {
        public int ExceptionsCount { get; }
        public List<Log> UnmatchedLogs { get; }
        public string DebugLogPath { get; }

        public ParsingResult(int exceptionsCount, List<Log> unmatchedLogs, string debugLogPath)
        {
            ExceptionsCount = exceptionsCount;
            UnmatchedLogs = unmatchedLogs;
            DebugLogPath = debugLogPath;
        }
    }
}
