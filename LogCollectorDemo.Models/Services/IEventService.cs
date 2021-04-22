using LogCollectorDemo.Core.Entities;
using LogCollectorDemo.Core.Models;
using System.Collections.Generic;

namespace LogCollectorDemo.Core.Services
{
    public interface IEventService
    {
        void SaveEvent(Log log1, Log log2);
        List<EventEntity> GetAll();
    }
}
