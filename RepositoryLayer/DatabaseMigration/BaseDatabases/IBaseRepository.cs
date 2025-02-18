namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
    public interface IBaseRepository
    {   
        Task OpenConnectionAsync();
        void OpenCommand(string commandText);

    }
}
