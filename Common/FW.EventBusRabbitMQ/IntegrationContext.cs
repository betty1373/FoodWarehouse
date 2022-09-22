using FW.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FW.EventBus
{
    public class IntegrationContext<TE> where TE : IIntegrationEvent
    {
        public string CorrelationId { get; set; }
        public TE Message { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public Func<object,Task> RespondAsync{ get; set; }
    }
}