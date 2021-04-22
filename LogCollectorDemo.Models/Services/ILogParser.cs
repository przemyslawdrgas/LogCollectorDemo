using LogCollectorDemo.Core.Models;
using System;

namespace LogCollectorDemo.Core.Services
{
    public interface ILogParser
    {
        ParsingResult Parse();
        event EventHandler LineProccesed;
        event EventHandler<MatchEventArgs> MatchFound;
        string LogFilePath { get; set; }
    }
}
