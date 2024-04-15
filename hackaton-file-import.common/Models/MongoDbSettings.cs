namespace hackaton_file_import.common.Models
{
    public class MongoDbSettings
    {
        public static string SectionName = "MongoDb";

        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
