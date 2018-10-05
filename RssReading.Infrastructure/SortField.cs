namespace RssReading.Infrastructure
{
    public class SortField
    {
        public string FieldName { get; set; }
        public SortDirection Direction { get; set; }
        public override string ToString()
        {
            return $"{FieldName} - {Direction}";
        }
    }
}