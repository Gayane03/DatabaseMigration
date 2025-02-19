using SharedLibrary.Enum;

namespace SharedLibrary.RequestModels
{
    public class ServerRequest
    {
        public ServerType ServerType { get; set; }
        public string? DatabaseConnection { get; set; }    
    }
}
