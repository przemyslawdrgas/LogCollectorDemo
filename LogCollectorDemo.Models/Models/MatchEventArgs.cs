using LogCollectorDemo.Core;
using System;

namespace LogCollectorDemo.Core.Models
{
    public class MatchEventArgs : EventArgs
    {
        public Log Log1 { get; set; }
        public Log Log2 { get; set; }
        public MatchEventArgs(Log log1, Log log2)
        {
            Log1 = log1;
            Log2 = log2;
        }
    }
}
