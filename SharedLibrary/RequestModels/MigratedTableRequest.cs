namespace SharedLibrary.RequestModels
{
	public class MigratedTableRequest
	{
		public string? TableName { get; set; }
		public bool IsContainFK { get; set; } = false;
	}
}
