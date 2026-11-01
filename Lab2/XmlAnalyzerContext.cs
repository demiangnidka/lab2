using System;
using System.Collections.Generic;

namespace Lab2
{
    public class XmlAnalyzerContext
    {
        private IXmlParserStrategy _strategy;

        public XmlAnalyzerContext(IXmlParserStrategy defaultStrategy)
        {
            _strategy = defaultStrategy;
        }

        public void SetStrategy(IXmlParserStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public List<ArchiveMaterial> ExecuteSearch(string xmlPath, SearchFilter filter)
        {
            if (string.IsNullOrWhiteSpace(xmlPath))
                return new List<ArchiveMaterial>();

            return _strategy.Search(xmlPath, filter);
        }
    }
}