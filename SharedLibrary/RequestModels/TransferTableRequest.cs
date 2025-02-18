using SharedLibrary.Enum;

namespace SharedLibrary.RequestModels
{
    public class TransferTableRequest
    {
        public string? FromTableName { get; set; }
        public ServerType FromServerType { get; set; }
        public string? FromServerConnection { get; set; }

        public ServerType ToServerType { get; set; }
        public string? ToServerConnection { get; set; }

        public bool IsContainFK { get; set; } = false;

    }
}
