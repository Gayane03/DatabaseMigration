﻿using BusinessLayer.Helper.ModelHelper;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services
{
    public interface IDataService
    {
        Task<Result<IEnumerable<MigratedTableResponse>>> GetTables(ServerRequest databaseRequest);
          
    }
}
