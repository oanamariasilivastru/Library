namespace LibraryConsole.Models
{
    public class BookSearchCriteria
    {
        // Filter by numeric ID
        public int?    Id             { get; set; }

        // Substring on title/author
        public string? TitleContains  { get; set; }
        public string? AuthorContains { get; set; }

        // Quantity range
        public int?    MinQuantity    { get; set; }
        public int?    MaxQuantity    { get; set; }
    }
}