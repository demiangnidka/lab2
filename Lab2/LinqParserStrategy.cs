using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lab2
{
    public class LinqParserStrategy : IXmlParserStrategy
    {
        public List<ArchiveMaterial> Search(string xmlPath, SearchFilter filter)
        {
            XDocument doc = XDocument.Load(xmlPath);

            List<ArchiveMaterial> results = (from element in doc.Descendants("Material")
                                             where MatchesElement(element, filter)
                                             select MapElementToMaterial(element)).ToList();

            return results;
        }

        private ArchiveMaterial MapElementToMaterial(XElement element)
        {
            var material = new ArchiveMaterial
            {
                Author = (string?)element.Attribute("Author") ?? string.Empty,
                Faculty = (string?)element.Attribute("Faculty") ?? string.Empty,
                Department = (string?)element.Attribute("Department") ?? string.Empty,
                MaterialType = (string?)element.Attribute("MaterialType") ?? string.Empty,
                Language = (string?)element.Attribute("Language") ?? string.Empty
            };

            XElement? detailsElement = element.Element("Details");
            if (detailsElement != null)
            {
                material.Details.Title = (string?)detailsElement.Element("Title") ?? string.Empty;
                material.Details.Volume = (string?)detailsElement.Element("Volume") ?? string.Empty;
                material.Details.CreationDate = (string?)detailsElement.Element("CreationDate") ?? string.Empty;
            }

            return material;
        }

        private bool MatchesElement(XElement element, SearchFilter filter)
        {
            if (filter == null || filter.IsEmpty())
                return true;

            ArchiveMaterial material = MapElementToMaterial(element);
            return filter.IsMatch(material);
        }
    }
}