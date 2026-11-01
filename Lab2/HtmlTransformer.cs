using System;
using System.IO;
using System.Xml.Xsl;

namespace Lab2
{
    public class HtmlTransformer
    {
        private XslCompiledTransform _xsltTransform;

        public HtmlTransformer()
        {
            _xsltTransform = new XslCompiledTransform();
        }

        public void TransformToHtml(string xmlPath, string xslPath, string htmlOutputPath)
        {
            if (!ValidateFiles(xmlPath, xslPath))
            {
                throw new FileNotFoundException("Не знайдено вхідний XML або XSL файл для трансформації.");
            }

            _xsltTransform.Load(xslPath);
            _xsltTransform.Transform(xmlPath, htmlOutputPath);
        }

        private bool ValidateFiles(string xmlPath, string xslPath)
        {
            if (string.IsNullOrWhiteSpace(xmlPath) || string.IsNullOrWhiteSpace(xslPath))
                return false;

            return File.Exists(xmlPath) && File.Exists(xslPath);
        }
    }
}