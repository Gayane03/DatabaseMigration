using System.Data.Common;

namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
    public abstract class BaseRepository
    {
        protected readonly string connectionPath;
        protected DbConnection? SqlConnection {  get; set; }
        protected DbCommand? SqlCommand { get; set; }
        protected DbDataReader? SqlReader { get; set; } 
        protected DbTransaction? SqlTransaction { get; set; }  

        protected BaseRepository(string connectionPath)
        {
           this.connectionPath = connectionPath;
        }

        public async Task OpenDataReaderAsync()
        {
            SqlReader = await SqlCommand!.ExecuteReaderAsync();
        }

        public async Task BeginTransactionAsync()
        {
            SqlTransaction = await SqlConnection!.BeginTransactionAsync();
        }

        public void Dispose()
        {
            SqlReader?.Dispose();
            SqlCommand?.Dispose();
            SqlTransaction?.Dispose();
            SqlConnection?.Dispose();
        }

    }
}
