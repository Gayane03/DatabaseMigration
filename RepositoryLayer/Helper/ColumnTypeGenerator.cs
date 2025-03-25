using SharedLibrary.Enum;

namespace RepositoryLayer.Helper
{
    public static class ColumnTypeGenerator
    {

        public static string GetColumnDataType(ServerType fromServerType, ServerType toServerType, string columnType)
        {
            columnType = columnType.ToLower(); 

            if (fromServerType == ServerType.MSSQL && toServerType == ServerType.PostgreSQL)
            {
                return columnType switch
                {
                    "int" => "INTEGER",
                    "bigint" => "BIGINT",
                    "smallint" => "SMALLINT",
                    "tinyint" => "SMALLINT",
                    "bit" => "BOOLEAN",
                    "decimal" or "numeric" => "DECIMAL",
                    "float" => "DOUBLE PRECISION",
                    "real" => "REAL",
                    "char" or "nchar" or "varchar" or "nvarchar" or "text" => "TEXT",
                    "date" => "DATE",
                    "datetime" or "smalldatetime" or "datetime2" => "TIMESTAMP",
                    "time" => "TIME",
                    "uniqueidentifier" => "UUID",
                    _ => "TEXT"
                };
            }
            else if (fromServerType == ServerType.MSSQL && toServerType == ServerType.MySQL)
            {
                return columnType switch
                {
                    "int" => "INT",
                    "bigint" => "BIGINT",
                    "smallint" => "SMALLINT",
                    "tinyint" => "TINYINT",
                    "bit" => "TINYINT(1)",
                    "decimal" or "numeric" => "DECIMAL",
                    "float" => "FLOAT",
                    "real" => "FLOAT",
                    "char" or "nchar" => "CHAR",
                    "varchar" or "nvarchar" => "VARCHAR(255)",
                    "text" => "TEXT",
                    "date" => "DATE",
                    "datetime" or "smalldatetime" or "datetime2" => "DATETIME",
                    "time" => "TIME",
                    "uniqueidentifier" => "CHAR(36)",
                    _ => "TEXT"
                };
            }
            else if (fromServerType == ServerType.PostgreSQL && toServerType == ServerType.MSSQL)
            {
                return columnType switch
                {
                    "integer" => "INT",
                    "bigint" => "BIGINT",
                    "smallint" => "SMALLINT",
                    "boolean" => "BIT",
                    "numeric" or "decimal" => "DECIMAL",
                    "double precision" => "FLOAT",
                    "real" => "REAL",
                    "text" => "VARCHAR(MAX)",
                    "char" or "character" => "CHAR",
                    "varchar" => "VARCHAR(255)",
                    "date" => "DATE",
                    "timestamp" => "DATETIME",
                    "time" => "TIME",
                    "uuid" => "UNIQUEIDENTIFIER",
                    _ => "VARCHAR(MAX)"
                };
            }
            else if (fromServerType == ServerType.MySQL && toServerType == ServerType.MSSQL)
            {
                return columnType switch
                {
                    "int" => "INT",
                    "bigint" => "BIGINT",
                    "smallint" => "SMALLINT",
                    "tinyint" => "TINYINT",
                    "bit" => "BIT",
                    "decimal" or "numeric" => "DECIMAL",
                    "float" => "FLOAT",
                    "double" => "DOUBLE PRECISION",
                    "char" => "CHAR",
                    "varchar" => "VARCHAR(255)",
                    "text" => "VARCHAR(MAX)",
                    "date" => "DATE",
                    "datetime" => "DATETIME",
                    "time" => "TIME",
                    "char(36)" => "UNIQUEIDENTIFIER",
                    _ => "VARCHAR(MAX)"
                };
            }

            return "TEXT"; 
        }



		public static string GetColumnDefaultValue(ServerType fromServerType, ServerType toServerType, string defaultValue, string columnType)
		{
			defaultValue = defaultValue?.Trim().Trim('(',')').ToLower();

			if (string.IsNullOrEmpty(defaultValue))
				return null;

			if (fromServerType == ServerType.MSSQL && toServerType == ServerType.PostgreSQL)
			{
				return defaultValue switch
				{
					"getdate" => "CURRENT_TIMESTAMP",
					"newid" => "gen_random_uuid()",
					"1" when columnType == "bit" || columnType == "boolean" => "TRUE",
					"0" when columnType == "bit" || columnType == "boolean" => "FALSE",
					_ => defaultValue
				};
			}
			else if (fromServerType == ServerType.MSSQL && toServerType == ServerType.MySQL)
			{
				return defaultValue switch
				{
					"getdate" => "CURRENT_TIMESTAMP",
					"newid" => "UUID()",
					"1" => "TRUE",
					"0" => "FALSE",
					_ => defaultValue
				};
			}
			else if (fromServerType == ServerType.PostgreSQL && toServerType == ServerType.MSSQL)
			{
				return defaultValue switch
				{
					"current_timestamp" => "GETDATE()",
					"gen_random_uuid" => "NEWID()",
					"true" => "1",
					"false" => "0",
					_ => defaultValue
				};
			}
			else if (fromServerType == ServerType.MySQL && toServerType == ServerType.MSSQL)
			{
				return defaultValue switch
				{
					"current_timestamp" => "GETDATE()",
					"uuid" => "NEWID()",
					"true" => "1",
					"false" => "0",
					_ => defaultValue
				};
			}

			return defaultValue;
		}


	}
}
