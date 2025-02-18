using Microsoft.Data.SqlClient;
using RepositoryLayer.DatabaseMigration;
using SharedLibrary.Enum;
using System.Reflection;
using System.Text;

namespace RepositoryLayer.Helper
{
	internal static class BaseRepositoryHelper
	{
		public static StringBuilder SetUpdatingParametersInQuery(StringBuilder updateQuery, Dictionary<string, object> updatingParameters)
		{
			if (updatingParameters.Count > 0)
			{
				var enumerator = updatingParameters.GetEnumerator();
				var hasMoreElement = enumerator.MoveNext();

				var currentElement = enumerator.Current;

				updateQuery.Append($"{currentElement.Key} = @{currentElement.Key} ");

				hasMoreElement = enumerator.MoveNext();

				while (hasMoreElement)
				{
					currentElement = enumerator.Current;

					updateQuery.Append($", {currentElement.Key} = @{currentElement.Key} ");
					hasMoreElement = enumerator.MoveNext();
				}
			}

			return updateQuery;
		}

		public static void GenerateParametersValues(Dictionary<string, object> parameters, SqlCommand sqlCommand)
		{
			foreach (var parameter in parameters)
			{
				sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
			}
		}

		public static string[] GetPropertyNames<TResult>()
		{
			return typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance)
								  .Select(p => p.Name)
								  .ToArray();
		}

		public static string GenerateTableSchemaQuery(string tableName,ServerType serverType)
		{
			return serverType switch
            {
                ServerType.MSSQL => $@"
               SELECT 
                   c.COLUMN_NAME, 
                   c.DATA_TYPE, 
                   c.CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                   CASE 
                       WHEN pk.COLUMN_NAME IS NOT NULL THEN 1 
                       ELSE 0 
                   END AS IsPrimaryKey,
                   CASE 
                       WHEN c.IS_NULLABLE = 'YES' THEN 1 
                       ELSE 0 
                   END AS IsNullable
               FROM INFORMATION_SCHEMA.COLUMNS c
               LEFT JOIN (
                   SELECT COLUMN_NAME 
                   FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
                   INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc 
                       ON kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
                   WHERE tc.TABLE_NAME = '{tableName}' 
                   AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
               ) pk ON c.COLUMN_NAME = pk.COLUMN_NAME
               WHERE c.TABLE_NAME = '{tableName}';",
                //ServerType.PostgreSQL => new PostgreSqlControlRepository(connectionString),
                // "mysql" => new MySQLHelper(connectionString),
                _ => throw new ArgumentException("Unsupported database type.")
            };
        }
	}
}
