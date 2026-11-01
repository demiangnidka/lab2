namespace Lab2
{
    public class ArchiveMaterial
    {
        public string Author { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public MaterialDetails Details { get; set; }

        public ArchiveMaterial()
        {
            Details = new MaterialDetails();
        }

        public string FormatForDisplay()
        {
            string lang = string.IsNullOrEmpty(Language) ? "Не вказано" : Language;

            return $"Автор: {Author} | Факультет: {Faculty} | Кафедра: {Department}\n" +
                   $"Вид матеріалу: {MaterialType} | Мова: {lang}\n" +
                   $"{Details.ToString()}\n" +
                   new string('-', 60);
        }
    }
}