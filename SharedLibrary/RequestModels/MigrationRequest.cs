namespace SharedLibrary.RequestModels
{
    public class MigrationRequest
    {        
        public ServerRequest FromDatabaseRequest { get; set; }
        public ServerRequest ToDatabaseRequest { get; set; }
        public List<MigratedTableRequest> MigratedTablesInfoRequest {  get; set; }  
	}
}
