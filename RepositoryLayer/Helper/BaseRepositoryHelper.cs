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

		public static string GenerateTableSchemaQuery(string tableName, ServerType serverType)
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
    END AS IsNullable,
    CASE 
        WHEN fk.TABLE_NAME IS NOT NULL THEN 1 
        ELSE 0 
    END AS IsForeignKey,
    fk.TABLE_NAME AS ForeignKeyTable,
    fk.COLUMN_NAME AS ForeignKeyColumn,
    CASE 
        WHEN uq.COLUMN_NAME IS NOT NULL THEN 1 
        ELSE 0 
    END AS IsUnique,
    COALESCE(c.COLUMN_DEFAULT, '') AS DefaultValue
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN (
    SELECT COLUMN_NAME 
    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
    INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc 
        ON kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
    WHERE tc.TABLE_NAME = '{tableName}' 
    AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
) pk ON c.COLUMN_NAME = pk.COLUMN_NAME
LEFT JOIN (
    SELECT 
        cu.COLUMN_NAME, 
        rc.UNIQUE_CONSTRAINT_NAME, 
        ku.TABLE_NAME
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE cu 
        ON rc.CONSTRAINT_NAME = cu.CONSTRAINT_NAME
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku 
        ON rc.UNIQUE_CONSTRAINT_NAME = ku.CONSTRAINT_NAME
) fk ON c.COLUMN_NAME = fk.COLUMN_NAME
LEFT JOIN (
    SELECT COLUMN_NAME 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu 
        ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
    WHERE tc.TABLE_NAME = '{tableName}' 
    AND tc.CONSTRAINT_TYPE = 'UNIQUE'
) uq ON c.COLUMN_NAME = uq.COLUMN_NAME
WHERE c.TABLE_NAME = '{tableName}';",
				_ => throw new ArgumentException("Unsupported database type.")
			};
		}



	}
}
