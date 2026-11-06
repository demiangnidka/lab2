using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Lab2
{
    public class HtmlTransformer
    {
        public void TransformToHtml(List<ArchiveMaterial> materials, string xslPath, string outputHtmlPath)
        {
            var root = new XElement("FacultyArchive");

            foreach (var m in materials)
            {
                var materialElement = new XElement("Material",
                    new XAttribute("Author", m.Author ?? string.Empty),
                    new XAttribute("Faculty", m.Faculty ?? string.Empty),
                    new XAttribute("Department", m.Department ?? string.Empty),
                    new XAttribute("MaterialType", m.MaterialType ?? string.Empty)
                );

                if (!string.IsNullOrEmpty(m.Language))
                {
                    materialElement.Add(new XAttribute("Language", m.Language));
                }

                materialElement.Add(new XElement("Details",
                    new XElement("Title", m.Details.Title ?? string.Empty),
                    new XElement("Volume", m.Details.Volume ?? string.Empty),
                    new XElement("CreationDate", m.Details.CreationDate ?? string.Empty)
                ));

                root.Add(materialElement);
            }

            XDocument filteredDoc = new XDocument(root);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslPath);

            using (XmlReader reader = filteredDoc.CreateReader())
            using (XmlWriter writer = XmlWriter.Create(outputHtmlPath, xslt.OutputSettings))
            {
                xslt.Transform(reader, writer);
            }
        }
    }
}