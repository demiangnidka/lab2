using System.Collections.Generic;
using System.Xml;

namespace Lab2
{
    public class SaxParserStrategy : IXmlParserStrategy
    {
        private XmlReaderSettings _readerSettings;

        public SaxParserStrategy()
        {
            _readerSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true
            };
        }

        public List<ArchiveMaterial> Search(string xmlPath, SearchFilter filter)    
        {
            var results = new List<ArchiveMaterial>();
            ArchiveMaterial? currentMaterial = null;

            using (XmlReader reader = XmlReader.Create(xmlPath, _readerSettings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Material")
                            {
                                currentMaterial = ReadMaterialAttributes(reader);
                            }
                            else if (reader.Name == "Details" && currentMaterial != null)
                            {
                                ReadMaterialDetails(reader, currentMaterial);
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == "Material" && currentMaterial != null)
                            {
                                if (filter == null || filter.IsMatch(currentMaterial))
                                {
                                    results.Add(currentMaterial);
                                }
                                currentMaterial = null;
                            }
                            break;
                    }
                }
            }

            return results;
        }

        private ArchiveMaterial ReadMaterialAttributes(XmlReader reader)
        {
            var material = new ArchiveMaterial();

            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    switch (reader.Name)
                    {
                        case "Author":
                            material.Author = reader.Value;
                            break;
                        case "Faculty":
                            material.Faculty = reader.Value;
                            break;
                        case "Department":
                            material.Department = reader.Value;
                            break;
                        case "MaterialType":
                            material.MaterialType = reader.Value;
                            break;
                        case "Language":
                            material.Language = reader.Value;
                            break;
                    }
                }
                reader.MoveToElement();
            }

            return material;
        }

        private void ReadMaterialDetails(XmlReader reader, ArchiveMaterial material)
        {
            string currentSubElement = string.Empty;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    currentSubElement = reader.Name;
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    switch (currentSubElement)
                    {
                        case "Title":
                            material.Details.Title = reader.Value;
                            break;
                        case "Volume":
                            material.Details.Volume = reader.Value;
                            break;
                        case "CreationDate":
                            material.Details.CreationDate = reader.Value;
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Details")
                {
                    break;
                }
            }
        }
    }
}