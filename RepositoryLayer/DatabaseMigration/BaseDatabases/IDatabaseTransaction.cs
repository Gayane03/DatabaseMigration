namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
	public interface IDatabaseTransaction
	{
		Task CommitTransaction();
		Task RollbackTransaction();
	}
}
