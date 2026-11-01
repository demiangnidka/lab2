using System;

namespace Lab2
{
    public class SearchFilter
    {
        public string Author { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public string Keyword { get; set; } = string.Empty;

        public SearchFilter()
        {
        }

        public bool IsMatch(ArchiveMaterial material)
        {
            if (material == null)
                return false;

            if (!string.IsNullOrEmpty(Author) && Author != "Усі" && material.Author != Author)
                return false;

            if (!string.IsNullOrEmpty(Faculty) && Faculty != "Усі" && material.Faculty != Faculty)
                return false;

            if (!string.IsNullOrEmpty(Department) && Department != "Усі" && material.Department != Department)
                return false;

            if (!string.IsNullOrEmpty(MaterialType) && MaterialType != "Усі" && material.MaterialType != MaterialType)
                return false;

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                string kw = Keyword.Trim();
                bool foundInAttributes =
                    material.Author.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    material.Faculty.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    material.Department.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    material.MaterialType.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    material.Language.Contains(kw, StringComparison.OrdinalIgnoreCase);

                bool foundInDetails =
                    material.Details.Title.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    material.Details.Volume.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    material.Details.CreationDate.Contains(kw, StringComparison.OrdinalIgnoreCase);

                if (!foundInAttributes && !foundInDetails)
                    return false;
            }

            return true;
        }

        public void Clear()
        {
            Author = string.Empty;
            Faculty = string.Empty;
            Department = string.Empty;
            MaterialType = string.Empty;
            Keyword = string.Empty;
        }

        public bool IsEmpty()
        {
            return (string.IsNullOrEmpty(Author) || Author == "Усі") &&
                   (string.IsNullOrEmpty(Faculty) || Faculty == "Усі") &&
                   (string.IsNullOrEmpty(Department) || Department == "Усі") &&
                   (string.IsNullOrEmpty(MaterialType) || MaterialType == "Усі") &&
                   string.IsNullOrWhiteSpace(Keyword);
        }
    }
}