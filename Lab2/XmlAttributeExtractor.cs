using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lab2
{
    public class XmlAttributeExtractor
    {
        private List<string> _targetAttributes;

        public XmlAttributeExtractor()
        {
            _targetAttributes = new List<string>
            {
                "Author",
                "Faculty",
                "Department",
                "MaterialType"
            };
        }

        public Dictionary<string, List<string>> ExtractUniqueValues(string xmlPath)
        {
            var result = new Dictionary<string, List<string>>();
            XDocument doc = XDocument.Load(xmlPath);

            foreach (string attrName in _targetAttributes)
            {
                result[attrName] = GetDistinctAttributeValues(doc, attrName);
            }

            return result;
        }

        private List<string> GetDistinctAttributeValues(XDocument doc, string attrName)
        {
            List<string> values = (from material in doc.Descendants("Material")
                                   let attr = material.Attribute(attrName)
                                   where attr != null && !string.IsNullOrWhiteSpace(attr.Value)
                                   select attr.Value)
                                  .Distinct()
                                  .OrderBy(v => v)
                                  .ToList();

            values.Insert(0, "Усі");
            return values;
        }
    }
}