using SharedLibrary.Enum;

namespace SharedLibrary.RequestModels
{
    public class DatabaseRequest
    {
        public ServerType Server { get; set; }
        public string? DatabaseConnection { get; set; }    
    }
}
