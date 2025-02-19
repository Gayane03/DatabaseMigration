using BusinessLayer.Services;
using BusinessLayer.Services.DatabaseMigrationHelper;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.RequestModels;

namespace BaseMigrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly IDataService dataService;
        private readonly IDatabaseMigrationService databaseMigrationService;

        public MigrationController(IDataService dataService, IDatabaseMigrationService databaseMigrationService)
        {
            this.dataService = dataService;
            this.databaseMigrationService = databaseMigrationService;
        }

        [HttpPost("getTables")]
        public async Task<IActionResult> GetDatabaseTables([FromBody] ServerRequest databaseRequest)
        {
            var result = await dataService.GetTables(databaseRequest);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("migrateTables")]
        public async Task<IActionResult> MigrateTables([FromBody] MigrationRequest migrationRequest)
        {
            var result = await databaseMigrationService.TryMigrateTables(migrationRequest);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
