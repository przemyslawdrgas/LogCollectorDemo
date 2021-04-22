using LogCollectorDemo.Core.Models;

namespace LogCollectorDemo.Core.Services
{
    public interface ILogService
    {
        public ILogParser LogsParser { get; }
        ParsingResult ProcessLogFile(string path);
    }
}
