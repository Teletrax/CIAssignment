using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FourC.Worker.Core;

namespace FourC.Worker.Backend
{
    public class DbService : IDbService
    {
        private readonly string _connectionString;

        public DbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveWorkAsync(WorkModel model, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Work VALUES(@User,@Content,@Timestamp)";
                    command.Parameters.AddWithValue("@User", model.User);
                    command.Parameters.AddWithValue("@Content", model.Content);
                    command.Parameters.AddWithValue("@Timestamp", model.Timestamp);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }
    }
}