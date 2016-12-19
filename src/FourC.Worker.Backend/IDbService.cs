using System.Threading;
using System.Threading.Tasks;
using FourC.Worker.Core;

namespace FourC.Worker.Backend
{
    public interface IDbService
    {
        Task SaveWorkAsync(WorkModel model, CancellationToken cancellationToken);
    }
}