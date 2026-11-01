namespace Lab2
{
    public class MaterialDetails
    {
        public string Title { get; set; } = string.Empty;
        public string Volume { get; set; } = string.Empty;
        public string CreationDate { get; set; } = string.Empty;

        public MaterialDetails()
        {
        }

        public override string ToString()
        {
            return $"Назва: \"{Title}\" | Обсяг: {Volume} | Дата: {CreationDate}";
        }
    }
}