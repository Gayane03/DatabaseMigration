namespace SharedLibrary.RequestModels
{
	public class MigratedTableRequest
	{
		public string? Name { get; set; }
		public bool IsContainFK { get; set; } = false;

		public List<string>? ReferencedTableNames { get; set; } = null;
	}


	public class MigratedTableResponse
	{
		public string? Name { get; set; }
		public bool IsContainFK { get; set; } = false;

		public List<string>? ReferencedTableNames { get; set; } = null;
	}
}
