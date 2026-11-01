using System.Collections.Generic;
using System.Xml;

namespace Lab2
{
    public class DomParserStrategy : IXmlParserStrategy
    {
        private XmlDocument _document;

        public DomParserStrategy()
        {
            _document = new XmlDocument();
        }

        public List<ArchiveMaterial> Search(string xmlPath, SearchFilter filter)
        {
            var results = new List<ArchiveMaterial>();
            _document.Load(xmlPath);
            
            XmlNodeList? materialNodes = _document.SelectNodes("//Material");
            if (materialNodes == null)
                return results;

            foreach (XmlNode node in materialNodes)
            {
                ArchiveMaterial material = ParseMaterialNode(node);

                if (filter == null || filter.IsMatch(material))
                {
                    results.Add(material);
                }
            }

            return results;
        }

        private ArchiveMaterial ParseMaterialNode(XmlNode node)
        {
            var material = new ArchiveMaterial
            {
                Author = GetAttributeValue(node, "Author"),
                Faculty = GetAttributeValue(node, "Faculty"),
                Department = GetAttributeValue(node, "Department"),
                MaterialType = GetAttributeValue(node, "MaterialType"),
                Language = GetAttributeValue(node, "Language")
            };

            XmlNode? detailsNode = node.SelectSingleNode("Details");
            if (detailsNode != null)
            {
                foreach (XmlNode child in detailsNode.ChildNodes)
                {
                    switch (child.Name)
                    {
                        case "Title":
                            material.Details.Title = child.InnerText;
                            break;
                        case "Volume":
                            material.Details.Volume = child.InnerText;
                            break;
                        case "CreationDate":
                            material.Details.CreationDate = child.InnerText;
                            break;
                    }
                }
            }

            return material;
        }

        private string GetAttributeValue(XmlNode node, string attrName)
        {
            if (node.Attributes == null)
                return string.Empty;

            XmlAttribute? attr = node.Attributes[attrName];
            return attr != null ? attr.Value : string.Empty;
        }
    }
}