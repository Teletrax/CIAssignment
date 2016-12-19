using System.Threading;
using System.Threading.Tasks;

namespace FourC.Worker.Core
{
    public interface IQueueService
    {
        Task<WorkModel> ReceiveAsync(CancellationToken cancellationToken);
        void Send(WorkModel data);
    }
}