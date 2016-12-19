using System.Messaging;

namespace FourC.Worker.Core
{
    public static class QueueFactory
    {
        public static MessageQueue CreateQueue(string name)
        {
            if (MessageQueue.Exists(name))
                return new MessageQueue(name);
            return MessageQueue.Create(name, true);
        }
    }
}