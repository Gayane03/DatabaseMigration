namespace SharedLibrary.DbModels
{
    public class ColumnInfo
    {
        public string? Name { get; set; }
        public string? DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public int? MaxLength { get; set; } 
    }
}
