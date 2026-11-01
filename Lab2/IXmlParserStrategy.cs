using System.Collections.Generic;

namespace Lab2
{
    public interface IXmlParserStrategy
    {
        List<ArchiveMaterial> Search(string xmlPath, SearchFilter filter);
    }
}