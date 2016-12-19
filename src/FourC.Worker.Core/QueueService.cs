using System.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace FourC.Worker.Core
{
    public class QueueService : IQueueService
    {
        private readonly MessageQueue _queue;
        private readonly IMessageFormatter _formatter;

        public QueueService(MessageQueue queue, IMessageFormatter formatter)
        {
            _queue = queue;
            _formatter = formatter;
        }

        public async Task<WorkModel> ReceiveAsync(CancellationToken cancellationToken)
        {
            var peekMessage = await Task.Factory.FromAsync(
                        _queue.BeginPeek(),
                        _queue.EndPeek)
                        .HandleCancellation(cancellationToken);

            var message = _queue.ReceiveById(peekMessage.Id, MessageQueueTransactionType.Automatic);
            message.Formatter = _formatter;
            return (WorkModel)message.Body;
        }

        public void Send(WorkModel data)
        {
            var message = new Message(data, _formatter);
            using (var transaction = new MessageQueueTransaction())
            {
                transaction.Begin();
                _queue.Send(message, transaction);
                transaction.Commit();
            }
        }
    }
}